using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using TvdbAnimeAbsoluteNumbering.Providers.tvdb;

namespace TvdbAnimeAbsoluteNumbering.Providers.AniDB.Metadata
{
    public class TvdbSeasonProvider : IRemoteMetadataProvider<Season, SeasonInfo>
    {
        private readonly IApplicationPaths _appPaths;
        private readonly IJsonSerializer _jsonSerializer;

        public TvdbSeasonProvider(IApplicationPaths appPaths, IJsonSerializer jsonSerializer)
        {
            _appPaths = appPaths;
            _jsonSerializer = jsonSerializer;
        }

        public string Name => "Add Anime Absolute Numbering";

        public async Task<MetadataResult<Season>> GetMetadata(SeasonInfo info, CancellationToken cancellationToken)
        {
            var seriesId = info.SeriesProviderIds["Tvdb"];
            AbsoluteNumberGenerator.GenerateAbsoluteEpisodeNumbers(seriesId, _appPaths.CachePath, _jsonSerializer);
            return null;
        }


        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeasonInfo searchInfo, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



    }
}