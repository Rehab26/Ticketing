using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Business.Encryption
{
    public static class Hashing
    {
        public static string Hash(this string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
        public static string ToString<T>(this IEnumerable<T> l, string separator)
        {
            return "[" + String.Join(separator, l.Select(i => i.ToString()).ToArray()) + "]";
        }
    }
}
