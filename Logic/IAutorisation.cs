using System.Threading.Tasks;
using MasterPassword;

namespace Logic
{
    public interface IAutorisation
    {
        Task<Encryption> LoginAsync(CancellationToken cancellationToken);
        Task<Encryption> SignUpAsync(CancellationToken cancellationToken);
    }
}