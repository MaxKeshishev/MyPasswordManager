using NUnit.Framework;

namespace Storage.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private string testDirectory = @"C:\Users\maxim\source\repos\MyPasswordManager\Files\";
        private string testFilePath;
        private Repository repository;

        [SetUp]
        public void Setup()
        {
            string login = "testUser";
            testFilePath = Path.Combine(testDirectory, login + ".txt");
            repository = new Repository(login);
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
            repository.CreateFile(CancellationToken.None);

            NUnit.Framework.Assert.IsTrue(File.Exists(testFilePath), "File was not created.");
        }

        [Test]
        public async Task WriteToFileAsync_ShouldWritePasswordToFile_WhenCalled()
        {
            string password = "password123";
            repository.CreateFile(CancellationToken.None);

            await repository.WriteToFileAsync(password, CancellationToken.None);

            string fileContents = await File.ReadAllTextAsync(testFilePath);
            NUnit.Framework.Assert.That(fileContents, Is.EqualTo(password), "Password was not written correctly.");
        }

        [Test]
        public async Task ReadFromFileAsync_ShouldReturnPassword_WhenCalled()
        {
            string password = "password123";
            repository.CreateFile(CancellationToken.None);
            await repository.WriteToFileAsync(password, CancellationToken.None);

            var result = await repository.ReadFromFileAsync(CancellationToken.None);

            NUnit.Framework.Assert.That(result, Is.EqualTo(password), "Password was not read correctly.");
        }

        [Test]
        public async Task WriteToFileAsync_ShouldAppendPasswordToFile_WhenFileExists()
        {
            string firstPassword = "password123";
            string secondPassword = "newPassword456";
            repository.CreateFile(CancellationToken.None);
            await repository.WriteToFileAsync(firstPassword, CancellationToken.None);

            await repository.WriteToFileAsync(secondPassword, CancellationToken.None);

            string fileContents = await File.ReadAllTextAsync(testFilePath);
            NUnit.Framework.Assert.That(fileContents, Is.EqualTo(secondPassword), "Password was not overwritten correctly.");
        }

        [Test]
        public void CreateFile_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.Throws<OperationCanceledException>(() => repository.CreateFile(cancellationToken));
        }

        [Test]
        public void WriteToFileAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => repository.WriteToFileAsync("password123", cancellationToken));
        }

        [Test]
        public void ReadFromFileAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => repository.ReadFromFileAsync(cancellationToken));
        }
    }
}
