using System.Globalization;
 
namespace WebBudgy.Models;
 
//for å omgjøre tall til fint text av norske kr.
public static class Money
{
    private static readonly CultureInfo Norsk = CultureInfo.GetCultureInfo("nb-NO");
 
    public static string Kr(decimal amount)
    {
        return amount.ToString("N2", Norsk) + " kr";
    }
}
