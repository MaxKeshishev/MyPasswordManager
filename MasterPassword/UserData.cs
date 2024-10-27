using System.Text;

namespace MasterPassword
{
    public class UserData : IUserData
    {
        private const string path = @"C:\Users\maxim\source\repos\MyPasswordManager\Files\MasterPassword\{0}.txt";
        private readonly string fullPath;

        public UserData(string login)
        {
            fullPath = string.Format(path, login);
        }

        public void CreateFile(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            File.WriteAllText(fullPath, string.Empty);
        }

        public async Task AddUserAsync(string master_password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await File.WriteAllTextAsync(fullPath, master_password, cancellationToken);
        }

        public async Task<bool> CheckUserAsync(string master_password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var storedPassword = await File.ReadAllTextAsync(fullPath, cancellationToken);
            return storedPassword == master_password;
        }
    }
}
