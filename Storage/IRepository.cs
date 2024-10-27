namespace Storage
{
    public interface IRepository
    {
        void CreateFile(CancellationToken cancellationToken);
        Task<string> ReadFromFileAsync(CancellationToken cancellationToken);
        Task WriteToFileAsync(string password, CancellationToken cancellationToken);
    }
}