
using System.Security.Cryptography;
using System.Text;

namespace AustellAcademyAdmissions.Helpers
{

public static class Utils
{
    public static string CalculateSignature(string message, string secret)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

}