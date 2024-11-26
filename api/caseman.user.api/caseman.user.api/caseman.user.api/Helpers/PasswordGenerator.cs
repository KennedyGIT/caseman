using System.Text;

namespace caseman.user.api.Helpers
{
    public class PasswordGenerator
    {
        private static readonly char[] UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] LowercaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] Digits = "0123456789".ToCharArray();
        private static readonly char[] SpecialCharacters = "!@#$%^&*()_+[]{}|;:,.<>?".ToCharArray();

        private static readonly char[] AllCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?".ToCharArray();
        private static readonly Random Random = new Random();

        public string GenerateStrongPassword(int length)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length should be at least 8 characters.");
            }

            var password = new StringBuilder();
            password.Append(UppercaseLetters[Random.Next(UppercaseLetters.Length)]);
            password.Append(LowercaseLetters[Random.Next(LowercaseLetters.Length)]);
            password.Append(Digits[Random.Next(Digits.Length)]);
            password.Append(SpecialCharacters[Random.Next(SpecialCharacters.Length)]);

            for (int i = 4; i < length; i++)
            {
                password.Append(AllCharacters[Random.Next(AllCharacters.Length)]);
            }

            return new string(password.ToString().ToCharArray().OrderBy(c => Random.Next()).ToArray());
        }
    }
}
