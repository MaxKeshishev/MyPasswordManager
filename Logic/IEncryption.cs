namespace Logic
{
    public interface IEncryption
    {
        Task<string> EncryptStringAsync(string plainText, CancellationToken cancellationToken);
        Task<string> DecryptStringAsync(string cipherText, CancellationToken cancellationToken);
    }
}