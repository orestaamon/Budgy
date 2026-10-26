using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace WebBudgy.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public int StudentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Velg en kategori")]
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }   // kommer fra TransactionView
    public string? Type { get; set; }           // "Income" eller "Expense"

    [Range(0.01, 1000000, ErrorMessage = "Beløpet må være større enn 0")]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(255)]
    public string? Description { get; set; }

    public bool RepeatMonthly { get; set; }


    // ---------- READ: alle transaksjonene til en student ----------
    public static List<Transaction> GetTransactions(string connectionString, int studentId)
    {
        var list = new List<Transaction>();
        string sql = "SELECT * FROM TransactionView WHERE StudentId = @StudentId " +
                     "ORDER BY [Date] DESC, TransactionId DESC";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        con.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            list.Add(ReadRow(dr));
        }
        return list;
    }

    // ---------- READ: én transaksjon (til Endre-siden) ----------
    public static Transaction? GetTransaction(string connectionString, int transactionId, int studentId)
    {
        string sql = "SELECT * FROM TransactionView " +
                     "WHERE TransactionId = @TransactionId AND StudentId = @StudentId";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@TransactionId", transactionId);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        con.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return ReadRow(dr);
        }
        return null;
    }

    // ---------- CREATE ----------
    public void NewTransaction(string connectionString)
    {
        string sql = "INSERT INTO TRANSACTIONS (StudentId, CategoryId, Amount, [Date], Description, RepeatMonthly) " +
                     "VALUES (@StudentId, @CategoryId, @Amount, @Date, @Description, @RepeatMonthly)";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        AddParameters(cmd);
        con.Open();
        cmd.ExecuteNonQuery();
    }

    // ---------- UPDATE ----------
    public void EditTransaction(string connectionString)
    {
        string sql = "UPDATE TRANSACTIONS " +
                     "SET CategoryId = @CategoryId, Amount = @Amount, [Date] = @Date, " +
                     "    Description = @Description, RepeatMonthly = @RepeatMonthly " +
                     "WHERE TransactionId = @TransactionId AND StudentId = @StudentId";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        AddParameters(cmd);
        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
        con.Open();
        cmd.ExecuteNonQuery();
    }

    // ---------- DELETE ----------
    public static void DeleteTransaction(string connectionString, int transactionId, int studentId)
    {
        string sql = "DELETE FROM TRANSACTIONS " +
                     "WHERE TransactionId = @TransactionId AND StudentId = @StudentId";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@TransactionId", transactionId);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        con.Open();
        cmd.ExecuteNonQuery();
    }

    // Hjelpemetode: verdiene sendes som parametere (beskytter mot SQL-injection)
    private void AddParameters(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@StudentId", StudentId);
        cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
        cmd.Parameters.AddWithValue("@Amount", Amount);
        cmd.Parameters.AddWithValue("@Date", Date);
        cmd.Parameters.AddWithValue("@Description", (object?)Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@RepeatMonthly", RepeatMonthly);
    }

    // Hjelpemetode: gjør én rad fra databasen om til et Transaction-objekt
    private static Transaction ReadRow(SqlDataReader dr)
    {
        return new Transaction
        {
            TransactionId = Convert.ToInt32(dr["TransactionId"]),
            StudentId = Convert.ToInt32(dr["StudentId"]),
            CategoryId = Convert.ToInt32(dr["CategoryId"]),
            CategoryName = dr["CategoryName"].ToString(),
            Type = dr["Type"].ToString(),
            Amount = Convert.ToDecimal(dr["Amount"]),
            Date = Convert.ToDateTime(dr["Date"]),
            Description = dr["Description"] == DBNull.Value ? null : dr["Description"].ToString(),
            RepeatMonthly = Convert.ToBoolean(dr["RepeatMonthly"])
        };
    }
}