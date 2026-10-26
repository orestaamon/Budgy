using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBudgy.Models;

namespace WebBudgy.Pages.Transactions;

public class IndexModel : PageModel
{
    private readonly string _connectionString;

    public IndexModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BudgyDb")!;
    }

    public List<Transaction> Transactions { get; set; } = new();

    // Kjøres når siden åpnes: hent alle transaksjoner (READ)
    public void OnGet()
    {
        Transactions = Transaction.GetTransactions(_connectionString, CurrentStudent.Id);
    }

    // Kjøres når "Slett" trykkes (DELETE)
    public IActionResult OnPostDelete(int id)
    {
        Transaction.DeleteTransaction(_connectionString, id, CurrentStudent.Id);
        return RedirectToPage("Index");
    }
}