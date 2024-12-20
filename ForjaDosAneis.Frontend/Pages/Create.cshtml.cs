using ForjaDosAneis.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Anel Anel { get; set; } = new();

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("AnelApi");

        var aneis = await client.GetFromJsonAsync<List<Anel>>("Aneis");
        var totalAtual = aneis.Count(a => a.Portador.Equals(Anel.Portador, StringComparison.OrdinalIgnoreCase));

        var limite = Anel.Portador.ToLower() switch
        {
            "elfos" => 3,
            "anões" => 7,
            "homens" => 9,
            "sauron" => 1,
            _ => int.MaxValue
        };

        if (totalAtual >= limite)
        {
            var errorMessage = $"O portador {Anel.Portador} já possui {totalAtual} anéis cadastrados. O limite é {limite} anéis.";
            ModelState.AddModelError(string.Empty, errorMessage);
            return Page();
        }

        HttpResponseMessage response;

        if (Anel.Id == 0)
        {
            response = await client.PostAsJsonAsync("Aneis", Anel);
        }
        else
        {
            response = await client.PutAsJsonAsync($"Aneis/{Anel.Id}", Anel);
        }

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return Page();
        }

        return RedirectToPage("Index");
    }
}