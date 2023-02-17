using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using TvdbAnimeAbsoluteNumbering.Providers.tvdb;

namespace TvdbAnimeAbsoluteNumbering.Providers.AniDB.Metadata
{
    public class TvdbSeriesProvider : IRemoteMetadataProvider<Series, SeriesInfo>, IHasOrder
    {
        private readonly IApplicationPaths _appPaths;
        private readonly IJsonSerializer _jsonSerializer;

        public TvdbSeriesProvider(IApplicationPaths appPaths, IJsonSerializer jsonSerializer)
        {
            _appPaths = appPaths;
            _jsonSerializer = jsonSerializer;

            Current = this;
        }

        internal static TvdbSeriesProvider Current { get; private set; }
        public int Order => 9;
        public string Name => "Add Anime Absolute Numbering";


        public async Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken cancellationToken)
        {
            var seriesId = info.GetProviderId("Tvdb");
            AbsoluteNumberGenerator.GenerateAbsoluteEpisodeNumbers(seriesId, _appPaths.CachePath, _jsonSerializer);
            return null;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // the tvdb plugin will do the heavy lifting
        }

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo searchInfo, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // the tvdb plugin will do the heavy lifting
        }
    }
}