using ForjaDosAneis.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Anel Anel { get; set; }

    public DeleteModel(IHttpClientFactory httpClientFactory)
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AnelApi");
        var response = await client.DeleteAsync($"Aneis/{id}");

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Erro ao excluir o anel.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}