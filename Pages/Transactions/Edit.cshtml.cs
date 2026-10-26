using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBudgy.Models;

namespace WebBudgy.Pages.Transactions;

public class EditModel : PageModel
{
    private readonly string _connectionString;

    public EditModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BudgyDb")!;
    }

    [BindProperty]
    public Transaction Transaction { get; set; } = new();

    public List<SelectListItem> CategoryList { get; set; } = new();

    // Henter transaksjonen og fyller skjemaet med de gamle verdiene
    public IActionResult OnGet(int id)
    {
        var existing = Models.Transaction.GetTransaction(_connectionString, id, CurrentStudent.Id);
        if (existing == null)
        {
            return NotFound();
        }

        Transaction = existing;
        LoadCategories();
        return Page();
    }

    // Kjøres når "Lagre" trykkes (UPDATE)
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadCategories();
            return Page();
        }

        Transaction.StudentId = CurrentStudent.Id;
        Transaction.EditTransaction(_connectionString);
        return RedirectToPage("Index");
    }

    private void LoadCategories()
    {
        CategoryList = Category.GetCategories(_connectionString, CurrentStudent.Id)
            .Select(c => new SelectListItem(
                c.Name + (c.Type == "Income" ? " (inntekt)" : " (utgift)"),
                c.CategoryId.ToString()))
            .ToList();
    }
}