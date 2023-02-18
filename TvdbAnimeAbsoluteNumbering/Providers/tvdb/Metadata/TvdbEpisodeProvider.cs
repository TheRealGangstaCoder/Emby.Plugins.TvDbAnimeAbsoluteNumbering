using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using TvdbAnimeAbsoluteNumbering.Providers.tvdb;
using TvdbAnimeAbsoluteNumbering.Providers.tvdb.Models;

namespace TvdbAnimeAbsoluteNumbering.Providers.AniDB.Metadata
{
    public class TvdbEpisodeProvider : IRemoteMetadataProvider<Episode, EpisodeInfo>
    {
        private readonly IApplicationPaths _appPaths;
        private readonly IJsonSerializer _jsonSerializer;

        public TvdbEpisodeProvider(IApplicationPaths appPaths, IJsonSerializer jsonSerializer)
        {
            _appPaths = appPaths;
            _jsonSerializer = jsonSerializer;
        }

        public string Name => "Add Anime Absolute Numbering";

        public async Task<MetadataResult<Episode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            var seriesId = info.SeriesProviderIds["Tvdb"];
            var seriesDataDirectory = Path.Combine(_appPaths.CachePath, "tvdb", seriesId);

            string searchPattern = $"episode-{info.ParentIndexNumber}-{info.IndexNumber}.json";

            var episodeFilePath = Directory.GetFiles(seriesDataDirectory, searchPattern).FirstOrDefault();
            var episodeFile = new FileInfo(episodeFilePath);

            if (!episodeFile.Exists)
            {
                AbsoluteNumberGenerator.GenerateAbsoluteEpisodeNumbers(seriesId, _appPaths.CachePath, _jsonSerializer);
                episodeFilePath = Directory.GetFiles(seriesDataDirectory, searchPattern).FirstOrDefault();
                episodeFile = new FileInfo(episodeFilePath);
            }

            var result = new MetadataResult<Episode>();

            if (episodeFile.Exists)
            {
                var episode = _jsonSerializer.DeserializeFromFile<TvdbEpisode>(episodeFile.FullName);
                result.Item = new Episode
                {
                    Name = episode.absolute_number.ToString().PadLeft(3, '0') + " - " + episode.name,
                    IndexNumber = info.IndexNumber,
                    ParentIndexNumber = info.ParentIndexNumber
                };

                result.HasMetadata = true;
                return result;
            }
            return null;
        }



        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // the tvdb plugin will do the heavy lifting
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // the tvdb plugin will do the heavy lifting
        }

    }
}