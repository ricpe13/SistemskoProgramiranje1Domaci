using Newtonsoft.Json;

public class Artwork
{
    public string title { get; set; }
    public string api_link { get; set; }

    public static string GetArtworks(string query)
    {
        query = query.Replace('_', ' ');

        if (Cache.cache.ContainsKey(query))
        {
            return Cache.cache[query];
        }

        try
        {
            string url = $"https://api.artic.edu/api/v1/artworks/search?q={query}&limit=100";
            string responseBody = HttpServer.client.DownloadString(url);
            var artworkResponse = JsonConvert.DeserializeObject<ArtworkResponse>(responseBody);
            if (artworkResponse.data.Count == 0)
            {
                return "<html><body>Greska: Nema umetnickih dela koja zadovoljavaju vasu pretragu.</body></html>";
            }
            string result = "<html><body>";
            foreach (var artwork in artworkResponse.data)
            {
                result += $"<p>{artwork.title}</p>";
            }
            result += "</body></html>";

            Cache.cache.Add(query, result);

            return result;
        }
        catch (Exception)
        {
            return "<html><body>Error.</body></html>";
        }
    }
}
