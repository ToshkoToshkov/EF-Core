namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albumsInfo = context
                .Albums
                .ToArray()
                .Where(a => a.ProducerId == producerId)
                .OrderByDescending(a => a.Price)
                .Select(e => new
                {
                    AlbumName =  e.Name,
                    ReleaseDate = e.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = e.Producer.Name,
                    Songs = e.Songs
                        .ToArray()
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("f2"),
                            Writer = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToArray(),
                    TotalAlbumPrice = e.Price.ToString("f2")
                })
                .ToArray();


            foreach (var item in albumsInfo)
            {
                sb
                    .AppendLine($"-AlbumName: {item.AlbumName}")
                    .AppendLine($"-ReleaseDate: {item.ReleaseDate}")
                    .AppendLine($"-ProducerName: {item.ProducerName}")
                    .AppendLine($"-Songs:");

                int i = 1;

                foreach (var song in item.Songs)
                {
                    sb
                        .AppendLine($"---#{i++}")
                        .AppendLine($"---SongName: {song.SongName}")
                        .AppendLine($"---Price: {song.Price}")
                        .AppendLine($"---Writer: {song.Writer}");
                }

                sb.AppendLine($"-AlbumPrice: {item.TotalAlbumPrice}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songsAboveDuration = context
                .Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,
                    Performer = s.SongPerformers
                            .ToArray()
                            .Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName)
                            .FirstOrDefault(),
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c", CultureInfo.InvariantCulture)
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToArray();

            int i = 1;

            foreach (var item in songsAboveDuration)
            {
                sb
                    .AppendLine($"-Song #{i++}")
                    .AppendLine($"---SongName: {item.SongName}")
                    .AppendLine($"---Writer: {item.Writer}")
                    .AppendLine($"---Performer: {item.Performer}")
                    .AppendLine($"---AlbumProducer: {item.AlbumProducer}")
                    .AppendLine($"---Duration: {item.Duration}");
            }

            return sb.ToString().TrimEnd();

        }
    }
}
