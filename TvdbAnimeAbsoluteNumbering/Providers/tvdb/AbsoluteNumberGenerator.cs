using System.IO;
using System.Linq;
using MediaBrowser.Model.Serialization;
using TvdbAnimeAbsoluteNumbering.Providers.tvdb.Models;

namespace TvdbAnimeAbsoluteNumbering.Providers.tvdb
{
    internal static class AbsoluteNumberGenerator
    {
        internal static void GenerateAbsoluteEpisodeNumbers(string seriesId, string cachePath, IJsonSerializer jsonSerializer)
        {
            string jsonDataFile = "episodes-official-eng.json";
            var seriesDataDirectory = Path.Combine(cachePath, "tvdb", seriesId);
            var episodesEngJson = File.ReadAllText(Path.Combine(seriesDataDirectory, jsonDataFile));

            var episodes = jsonSerializer.DeserializeFromString<TvdbEpisodes>(episodesEngJson);

            int i = 0;
            foreach (var episode in episodes.episodes.Where(x => x.seasonNumber != 0 & !x.isMovie).OrderBy(x => x.seasonNumber).ThenBy(x => x.number))
            {
                i++;
                if (seriesId == "81797" && i == 590)
                {
                    i++; // skip 590, as it is a special episode
                }

                episode.absolute_number = i;

                var file = Path.Combine(seriesDataDirectory, string.Format("episode-{0}-{1}.json", episode.seasonNumber, episode.number));
                jsonSerializer.SerializeToFile(episode, file);

            }
        }
    }
}
