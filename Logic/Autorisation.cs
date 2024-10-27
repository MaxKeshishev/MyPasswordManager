using MasterPassword;

namespace Logic
{
    public class Autorisation : IAutorisation
    {
        private string login, masterPassword;

        public Autorisation(string userLogin, string userMasterPassword)
        {
            login = userLogin;
            masterPassword = userMasterPassword;
        }

        public async Task<Encryption> LoginAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var encryption = new Encryption(masterPassword + login);
            var userData = new UserData(login);

            var masterPasswordCopy = await encryption.EncryptStringAsync(masterPassword, cancellationToken);
            var answer = await userData.CheckUserAsync(masterPasswordCopy, cancellationToken);

            if (answer == false)
                encryption = null;

            return encryption;
        }

        public async Task<Encryption> SignUpAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var encryption = new Encryption(masterPassword + login);

            var userData = new UserData(login);

            userData.CreateFile(cancellationToken);

            var masterPasswordCopy = await encryption.EncryptStringAsync(masterPassword, cancellationToken);
            await userData.AddUserAsync(masterPasswordCopy, cancellationToken);

            return encryption;
        }
    }
}
