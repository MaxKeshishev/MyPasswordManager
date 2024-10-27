using System.Threading.Tasks;

namespace Logic
{
    public interface IWorkWithRepository
    {
        Task AddPasswordAsync(string password, CancellationToken cancellationToken);
        Task<string> FindPasswordAsync(CancellationToken cancellationToken);
        Task ChangePasswordAsync(string password, CancellationToken cancellationToken);
    }
}