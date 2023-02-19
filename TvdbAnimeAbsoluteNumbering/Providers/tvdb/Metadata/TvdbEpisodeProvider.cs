using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Logging;
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
        private readonly ILogger _logger;

        public TvdbEpisodeProvider(IApplicationPaths appPaths, IJsonSerializer jsonSerializer, ILogger logger)
        {
            _appPaths = appPaths;
            _jsonSerializer = jsonSerializer;
            _logger = logger;
    }

    public string Name => "Add Anime Absolute Numbering";

        public async Task<MetadataResult<Episode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Episode>();
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogSeverity.Error, ex.Message, ex);
                return result;
            }
            return result;
        }



        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken)
        {
            var list = new List<RemoteSearchResult>();

            try
            {
                var metadataResult = await GetMetadata(searchInfo, cancellationToken).ConfigureAwait(false);

                if (metadataResult.HasMetadata)
                {
                    var item = metadataResult.Item;

                    list.Add(new RemoteSearchResult
                    {
                        IndexNumber = item.IndexNumber,
                        Name = item.Name,
                        ParentIndexNumber = item.ParentIndexNumber,
                    });
                }
            }
            catch (FileNotFoundException)
            {
                // Don't fail the provider because this will just keep on going and going.
            }
            catch (DirectoryNotFoundException)
            {
                // Don't fail the provider because this will just keep on going and going.
            }

            return list;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // the tvdb plugin will do the heavy lifting
        }

    }
}