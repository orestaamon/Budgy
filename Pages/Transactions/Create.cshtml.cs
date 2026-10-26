using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBudgy.Models;

namespace WebBudgy.Pages.Transactions;

public class CreateModel : PageModel
{
    private readonly string _connectionString;

    public CreateModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BudgyDb")!;
    }

    [BindProperty]
    public Transaction Transaction { get; set; } = new();

    public List<SelectListItem> CategoryList { get; set; } = new();

    // Viser det tomme skjemaet
    public void OnGet()
    {
        LoadCategories();
    }

    // Kjøres når "Lagre" trykkes (CREATE)
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadCategories();
            return Page();          // vis skjemaet på nytt med feilmeldinger
        }

        Transaction.StudentId = CurrentStudent.Id;
        Transaction.NewTransaction(_connectionString);
        return RedirectToPage("Index");
    }

    // Fyller nedtrekkslisten med kategorier fra databasen
    private void LoadCategories()
    {
        CategoryList = Category.GetCategories(_connectionString, CurrentStudent.Id)
            .Select(c => new SelectListItem(
                c.Name + (c.Type == "Income" ? " (inntekt)" : " (utgift)"),
                c.CategoryId.ToString()))
            .ToList();
    }
}