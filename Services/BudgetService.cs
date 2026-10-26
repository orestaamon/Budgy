using Microsoft.Data.SqlClient;

namespace WebBudgy.Services;

// Én linje i "Utgifter per kategori"
public class CategorySum
{
    public string CategoryName { get; set; } = "";
    public decimal Total { get; set; }
}

public class BudgetService
{
    private readonly string _connectionString;

    public BudgetService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Totale inntekter og utgifter for én måned (SUM + GROUP BY)
    public (decimal Income, decimal Expenses) GetMonthlySummary(int studentId, int year, int month)
    {
        decimal income = 0;
        decimal expenses = 0;

        string sql = "SELECT Type, SUM(Amount) AS Total " +
                     "FROM TransactionView " +
                     "WHERE StudentId = @StudentId AND YEAR([Date]) = @Year AND MONTH([Date]) = @Month " +
                     "GROUP BY Type";

        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);
        con.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            if (dr["Type"].ToString() == "Income")
                income = Convert.ToDecimal(dr["Total"]);
            else
                expenses = Convert.ToDecimal(dr["Total"]);
        }
        return (income, expenses);
    }

    // Hvor mye som er brukt i hver kategori én måned
    public List<CategorySum> GetSpentPerCategory(int studentId, int year, int month)
    {
        var list = new List<CategorySum>();

        string sql = "SELECT CategoryName, SUM(Amount) AS Total " +
                     "FROM TransactionView " +
                     "WHERE StudentId = @StudentId AND Type = 'Expense' " +
                     "AND YEAR([Date]) = @Year AND MONTH([Date]) = @Month " +
                     "GROUP BY CategoryName " +
                     "ORDER BY Total DESC";

        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);
        con.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            list.Add(new CategorySum
            {
                CategoryName = dr["CategoryName"].ToString() ?? "",
                Total = Convert.ToDecimal(dr["Total"])
            });
        }
        return list;
    }
}