using System;
using System.Collections.Generic;
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

namespace TvdbAnimeAbsoluteNumbering.Providers.AniDB.Metadata
{
    public class TvdbSeasonProvider : IRemoteMetadataProvider<Season, SeasonInfo>
    {
        private readonly IApplicationPaths _appPaths;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger _logger;

        public TvdbSeasonProvider(IApplicationPaths appPaths, IJsonSerializer jsonSerializer, ILogger loger)
        {
            _appPaths = appPaths;
            _jsonSerializer = jsonSerializer;
            _logger = loger;
        }

        public string Name => "Add Anime Absolute Numbering";

        public async Task<MetadataResult<Season>> GetMetadata(SeasonInfo info, CancellationToken cancellationToken)
        {
            try
            {
                var seriesId = info.SeriesProviderIds["Tvdb"];
                AbsoluteNumberGenerator.GenerateAbsoluteEpisodeNumbers(seriesId, _appPaths.CachePath, _jsonSerializer);
            }
            catch (Exception ex)
            {
                _logger.Log(LogSeverity.Error, ex.Message, ex);
            }
            var result = new MetadataResult<Season>
            {
                HasMetadata = true,
                Item = new Season
                {
                    Name = info.Name,
                    IndexNumber = info.IndexNumber
                }
            };
            return result;
        }


        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeasonInfo searchInfo, CancellationToken cancellationToken)
        {
            var metadata = await GetMetadata(searchInfo, cancellationToken).ConfigureAwait(false);

            var list = new List<RemoteSearchResult>();

            if (metadata.HasMetadata)
            {
                var res = new RemoteSearchResult
                {
                    Name = metadata.Item.Name,
                };

                list.Add(res);
            }

            return list;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



    }
}