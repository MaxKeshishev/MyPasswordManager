using System.Security.Cryptography;
using System.Text;

namespace Logic
{
    public class Encryption : IEncryption
    {
        private const string V = "LbN4izbORyAls8Hh78xrYDISDcs0x1UO";
        private const string S = "1A2B3C4D5E6F7G8H";
        private byte[] key;
        private byte[] iv = Encoding.UTF8.GetBytes(S);

        public Encryption(string newKey)
        {
            string n = V;
            key = Encoding.UTF8.GetBytes(newKey + n.Substring(newKey.Length));
        }

        public async Task<string> EncryptStringAsync(string plainText, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        await swEncrypt.WriteAsync(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public async Task<string> DecryptStringAsync(string cipherText, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        var result = await srDecrypt.ReadToEndAsync();
                        return result;
                    }
                }
            }
            catch (CryptographicException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
