namespace Storage
{
    public class Repository : IRepository
    {
        private const string path = @"C:\Users\maxim\source\repos\MyPasswordManager\Files\{0}.txt";
        private readonly string fullPath;

        public Repository(string login)
        {
            fullPath = string.Format(path, login);
        }

        public void CreateFile(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            File.WriteAllText(fullPath, string.Empty);
        }

        public async Task<string> ReadFromFileAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await File.ReadAllTextAsync(fullPath, cancellationToken);

            return result;
        }

        public async Task WriteToFileAsync(string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await File.WriteAllTextAsync(fullPath, password, cancellationToken);
        }
    }
}
