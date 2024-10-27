namespace MasterPassword
{
    public interface IUserData
    {
        void CreateFile(CancellationToken cancellationToken);
        Task AddUserAsync(string masterPassword, CancellationToken cancellationToken);
        Task<bool> CheckUserAsync(string masterPassword, CancellationToken cancellationToken);
    }
}