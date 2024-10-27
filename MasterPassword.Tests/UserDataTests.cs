using NUnit.Framework;

namespace MasterPassword.Tests
{
    [TestFixture]
    public class UserDataTests
    {
        private string testDirectory = @"C:\Users\maxim\source\repos\MyPasswordManager\Files\MasterPassword\";
        private string testFilePath;
        private UserData userData;

        [SetUp]
        public void Setup()
        {
            string login = "testUser";
            testFilePath = Path.Combine(testDirectory, login + ".txt");
            userData = new UserData(login);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [Test]
        public void CreateFile_ShouldCreateFile_WhenCalled()
        {
            userData.CreateFile(CancellationToken.None);

            NUnit.Framework.Assert.IsTrue(File.Exists(testFilePath), "File was not created.");
        }

        [Test]
        public async Task AddUserAsync_ShouldWriteMasterPasswordToFile_WhenCalled()
        {
            string masterPassword = "password123";
            userData.CreateFile(CancellationToken.None);

            await userData.AddUserAsync(masterPassword, CancellationToken.None);

            string fileContents = await File.ReadAllTextAsync(testFilePath);
            NUnit.Framework.Assert.That(fileContents, Is.EqualTo(masterPassword), "Master password was not written correctly.");
        }

        [Test]
        public async Task CheckUserAsync_ShouldReturnTrue_WhenPasswordMatches()
        {
            string masterPassword = "password123";
            userData.CreateFile(CancellationToken.None);
            await userData.AddUserAsync(masterPassword, CancellationToken.None);

            bool result = await userData.CheckUserAsync(masterPassword, CancellationToken.None);

            NUnit.Framework.Assert.IsTrue(result, "CheckUserAsync did not return true for the correct password.");
        }

        [Test]
        public async Task CheckUserAsync_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            string masterPassword = "password123";
            string wrongPassword = "wrongPassword";
            userData.CreateFile(CancellationToken.None);
            await userData.AddUserAsync(masterPassword, CancellationToken.None);

            bool result = await userData.CheckUserAsync(wrongPassword, CancellationToken.None);

            NUnit.Framework.Assert.IsFalse(result, "CheckUserAsync did not return false for the incorrect password.");
        }

        [Test]
        public void CreateFile_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.Throws<OperationCanceledException>(() => userData.CreateFile(cancellationToken));
        }

        [Test]
        public void AddUserAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => userData.AddUserAsync("password123", cancellationToken));
        }

        [Test]
        public void CheckUserAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => userData.CheckUserAsync("password123", cancellationToken));
        }
    }
}
