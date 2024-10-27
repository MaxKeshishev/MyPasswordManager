using Storage;

namespace Logic
{
    public class WorkWithRepository : IWorkWithRepository
    {
        private IRepository repository;
        private IEncryption encryption;

        public WorkWithRepository(IRepository _repository, IEncryption _encryption)
        {
            repository = _repository;
            encryption = _encryption;
        }

        public async Task AddPasswordAsync(string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            repository.CreateFile(cancellationToken);
            password = await encryption.EncryptStringAsync(password, cancellationToken);
            await repository.WriteToFileAsync(password, cancellationToken);
        }

        public async Task<string> FindPasswordAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fromFile = await repository.ReadFromFileAsync(cancellationToken);
            var result = await encryption.DecryptStringAsync(fromFile, cancellationToken);
            return result;
        }

        public async Task ChangePasswordAsync(string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            password = await encryption.EncryptStringAsync(password, cancellationToken);
            await repository.WriteToFileAsync(password, cancellationToken);
        }
    }
}
