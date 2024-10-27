using NUnit.Framework;

namespace Logic.Tests
{
    [TestFixture]
    public class AutorisationTests
    {
        private string login = "testUser";
        private string masterPassword = "password123";
        private Autorisation _autorisation;
        private string userFilePath;
        private string masterPasswordFilePath;

        [SetUp]
        public void Setup()
        {
            _autorisation = new Autorisation(login, masterPassword);
            userFilePath = $@"C:\Users\maxim\source\repos\MyPasswordManager\Files\MasterPassword\{login}.txt";
            masterPasswordFilePath = $@"C:\Users\maxim\source\repos\MyPasswordManager\Files\{login}.txt";
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(userFilePath))
            {
                File.Delete(userFilePath);
            }

            if (File.Exists(masterPasswordFilePath))
            {
                File.Delete(masterPasswordFilePath);
            }
        }

        [Test]
        public async Task LoginAsync_ShouldReturnEncryption_WhenLoginIsSuccessful()
        {
            var signUpEncryption = await _autorisation.SignUpAsync(CancellationToken.None);

            var loginEncryption = await _autorisation.LoginAsync(CancellationToken.None);

            NUnit.Framework.Assert.IsNotNull(loginEncryption);
        }

        [Test]
        public async Task LoginAsync_ShouldReturnNull_WhenLoginFails()
        {
            var signUpEncryption = await _autorisation.SignUpAsync(CancellationToken.None);

            var invalidAutorisation = new Autorisation(login, "wrongPassword");

            var result = await invalidAutorisation.LoginAsync(CancellationToken.None);

            NUnit.Framework.Assert.IsNull(result);
        }

        [Test]
        public async Task SignUpAsync_ShouldReturnEncryption_WhenSignUpIsSuccessful()
        {
            var result = await _autorisation.SignUpAsync(CancellationToken.None);

            NUnit.Framework.Assert.IsNotNull(result);
            NUnit.Framework.Assert.IsTrue(File.Exists(userFilePath));
        }

        [Test]
        public void LoginAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => _autorisation.LoginAsync(cancellationToken));
        }

        [Test]
        public void SignUpAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => _autorisation.SignUpAsync(cancellationToken));
        }
    }
}
