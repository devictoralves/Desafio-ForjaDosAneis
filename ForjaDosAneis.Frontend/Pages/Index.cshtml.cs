using ForjaDosAneis.Frontend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public List<Anel> Aneis { get; set; } = new();
    public Dictionary<string, (int Total, int Limite)> Portadores { get; set; } = new();

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("AnelApi");
        Aneis = await client.GetFromJsonAsync<List<Anel>>("Aneis");

        var portadores = new Dictionary<string, int>
        {
            { "Elfos", 3 },
            { "Anões", 7 },
            { "Homens", 9 },
            { "Sauron", 1 }
        };

        foreach (var portador in portadores.Keys)
        {
            var total = Aneis.Count(a => a.Portador.Equals(portador, StringComparison.OrdinalIgnoreCase));
            Portadores[portador] = (total, portadores[portador] - total);
        }
    }
}