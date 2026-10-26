using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBudgy.Models;
using WebBudgy.Services;

namespace WebBudgy.Pages;

public class IndexModel : PageModel
{
    private readonly string _connectionString;

    public IndexModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BudgyDb")!;
    }

    public DateTime SelectedMonth { get; set; }
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
    public decimal Left => Income - Expenses;
    public List<CategorySum> SpentPerCategory { get; set; } = new();

    public string MonthName =>
        SelectedMonth.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("nb-NO"));

    // year og month kommer fra adressen, f.eks. /?year=2026&month=10
    public void OnGet(int? year, int? month)
    {
        SelectedMonth = new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1);

        var service = new BudgetService(_connectionString);
        (Income, Expenses) = service.GetMonthlySummary(CurrentStudent.Id, SelectedMonth.Year, SelectedMonth.Month);
        SpentPerCategory = service.GetSpentPerCategory(CurrentStudent.Id, SelectedMonth.Year, SelectedMonth.Month);
    }
}