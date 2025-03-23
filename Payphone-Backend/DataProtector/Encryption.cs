using System.Security.Cryptography;
using System.Text;

namespace DataProtector;

public class Encryption
    {
        #region Private Members
        private const string IV = "0bFQw4JDWseLLJKH/fy9sg==";
        private const string _DefaultCryptoKey = "Gm/nVc71SldM7YDwUa+/sQ==";
        #endregion

        private static Aes CreateCipher(string keyBase64)
        {
            var cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);
            return cipher;
        }

		/// <summary>
		///     Create new IV from a key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static string NewIVBase64String(string key) => Convert.ToBase64String(CreateCipher(key).IV);

		/// <summary>
		///     Create new Random Key
		/// </summary>
		/// <param name="length"></param>
		/// <returns>Base64 string</returns>
		private static string GetRandomKey(int length)
		{
			using var provider = RandomNumberGenerator.Create();
			var byteArray = new byte[length];
			provider.GetBytes(byteArray);
			var base64 = Convert.ToBase64String(byteArray);
			return base64;
		}

        public static string Encrypt(string rawText)
        {
            try
            {
                var cipher = CreateCipher(_DefaultCryptoKey);
                cipher.IV = Convert.FromBase64String(IV);

                var cryptTransform = cipher.CreateEncryptor();
                var plaintext = Encoding.UTF8.GetBytes(rawText);
                var cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

                return Convert.ToBase64String(cipherText);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Decrypt(string? encryptedString)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedString)) return string.Empty;

				var cipher = CreateCipher(_DefaultCryptoKey);
                cipher.IV = Convert.FromBase64String(IV);

                var cryptTransform = cipher.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64String(encryptedString.Trim());
                var plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(plainBytes);

			}
            catch
            {
	            return string.Empty;
            }
        }
	}