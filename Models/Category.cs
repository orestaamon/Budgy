using Microsoft.Data.SqlClient;

namespace WebBudgy.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";   // "Income" eller "Expense"

    // READ: faste kategorier + studentens egne
    public static List<Category> GetCategories(string connectionString, int studentId)
    {
        var categories = new List<Category>();
        string sql = "SELECT CategoryId, Name, Type FROM CATEGORY " +
                     "WHERE StudentId IS NULL OR StudentId = @StudentId " +
                     "ORDER BY Type DESC, Name";

        using SqlConnection con = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        con.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            categories.Add(new Category
            {
                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                Name = dr["Name"].ToString() ?? "",
                Type = dr["Type"].ToString() ?? ""
            });
        }
        return categories;
    }
}