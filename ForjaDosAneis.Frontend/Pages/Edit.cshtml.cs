using ForjaDosAneis.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Anel Anel { get; set; }

    public EditModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AnelApi");
        Anel = await client.GetFromJsonAsync<Anel>($"Aneis/{id}");
        if (Anel == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("AnelApi");

        var aneis = await client.GetFromJsonAsync<List<Anel>>("Aneis");
        var totalAtual = aneis.Count(a => a.Portador.Equals(Anel.Portador, StringComparison.OrdinalIgnoreCase) && a.Id != Anel.Id);

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

        var response = await client.PutAsJsonAsync($"Aneis/{Anel.Id}", Anel);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
