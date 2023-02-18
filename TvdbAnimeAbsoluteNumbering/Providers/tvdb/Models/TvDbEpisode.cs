using System;
using System.Collections.Generic;
using System.Text;

namespace TvdbAnimeAbsoluteNumbering.Providers.tvdb.Models
{
    public class TvdbEpisode
    {
        public int id { get; set; }
        public int seriesId { get; set; }
        public string name { get; set; }
        public string aired { get; set; }
        public int runtime { get; set; }
        public string overview { get; set; }
        public string image { get; set; }
        public int imageType { get; set; }
        public bool isMovie { get; set; }
        public int absolute_number { get; set; }
        public int number { get; set; }
        public int seasonNumber { get; set; }
        public string lastUpdated { get; set; }
        public int airsBeforeSeason { get; set; }
        public int airsBeforeEpisode { get; set; }
        public int? airsAfterSeason { get; set; }
        public string finaleType { get; set; }
    }

    public class TvdbEpisodes
    {
        public List<object> characters { get; set; }
        public List<TvdbEpisode> episodes { get; set; }
    }


}
