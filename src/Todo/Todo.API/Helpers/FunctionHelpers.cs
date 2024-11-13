using Konscious.Security.Cryptography;
using System.Text;

namespace Todo.API.Helpers
{
    public class FunctionHelpers
    {
        public static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,  
                Iterations = 4,           
                MemorySize = 65536 
            };

            byte[] hash = argon2.GetBytes(32);

            string hashBase64 = Convert.ToBase64String(hash);
            string saltBase64 = Convert.ToBase64String(salt);

            return (hashBase64, saltBase64);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashBase64, string storedSaltBase64)
        {
            
            byte[] storedHash = Convert.FromBase64String(storedHashBase64);
            byte[] storedSalt = Convert.FromBase64String(storedSaltBase64);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(enteredPassword))
            {
                Salt = storedSalt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };

            byte[] hashToVerify = argon2.GetBytes(32);

            return AreHashesEqual(storedHash, hashToVerify);
        }

        private static bool AreHashesEqual(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }
    }
}
