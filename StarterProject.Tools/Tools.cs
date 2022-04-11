using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Globalization;
using System.Security.Cryptography;

namespace StarterProject.Crosscutting
{
    public static class Tools
    {
        public static string Capitalize(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(text);
        }

        public static string EncryptPassword(string password, out byte[] salt)
        {
            salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return encryptedPassw;
        }

        public static bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return encryptedPassw == storedPassword;
        }
    }
}