using System.Security.Cryptography;

namespace DataProtector;

public class PasswordGenerator
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*";
    private const string AllCharacters = Lowercase + Uppercase + Numbers + SpecialCharacters;

    public static string GeneratePassword(int length = 8)
    {
        if (length < 4) // Ensure minimum length to include all character types
        {
            throw new ArgumentException("Password length must be at least 4 characters.");
        }

        var randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Ensure the password includes at least one character of each type
        var passwordChars = new char[length];
        passwordChars[0] = Lowercase[randomBytes[0] % Lowercase.Length];
        passwordChars[1] = Uppercase[randomBytes[1] % Uppercase.Length];
        passwordChars[2] = Numbers[randomBytes[2] % Numbers.Length];
        passwordChars[3] = SpecialCharacters[randomBytes[3] % SpecialCharacters.Length];

        // Fill the rest of the password with random characters from all types
        for (var i = 4; i < length; i++)
        {
            passwordChars[i] = AllCharacters[randomBytes[i] % AllCharacters.Length];
        }

        // Shuffle the constructed password to randomize character distribution
        return new string(passwordChars.OrderBy(_ => Guid.NewGuid()).ToArray());
    }
}