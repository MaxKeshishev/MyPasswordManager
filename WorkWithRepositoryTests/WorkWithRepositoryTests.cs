using Storage;
using Moq;
using NUnit.Framework;

namespace Logic.Tests
{
    [TestFixture]
    public class WorkWithRepositoryTests
    {
        private Mock<IRepository> _mockRepository;
        private Mock<IEncryption> _mockEncryption;
        private WorkWithRepository _workWithRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository>();
            _mockEncryption = new Mock<IEncryption>();

            _workWithRepository = new WorkWithRepository(_mockRepository.Object, _mockEncryption.Object);
        }

        [Test]
        public async Task AddPasswordAsync_ShouldEncryptPasswordAndWriteToFile()
        {
            string password = "myPassword";
            string encryptedPassword = "encryptedPassword";
            CancellationToken cancellationToken = CancellationToken.None;

            _mockEncryption.Setup(e => e.EncryptStringAsync(password, cancellationToken)).ReturnsAsync(encryptedPassword);

            await _workWithRepository.AddPasswordAsync(password, cancellationToken);

            _mockRepository.Verify(r => r.CreateFile(cancellationToken), Times.Once);
            _mockRepository.Verify(r => r.WriteToFileAsync(encryptedPassword, cancellationToken), Times.Once);
        }

        [Test]
        public async Task FindPasswordAsync_ShouldReturnDecryptedPassword()
        {
            string encryptedPassword = "encryptedPassword";
            string decryptedPassword = "myPassword";
            CancellationToken cancellationToken = CancellationToken.None;

            _mockRepository
                .Setup(r => r.ReadFromFileAsync(cancellationToken))
                .ReturnsAsync(encryptedPassword);

            _mockEncryption
                .Setup(e => e.DecryptStringAsync(encryptedPassword, cancellationToken))
                .ReturnsAsync(decryptedPassword);

            var result = await _workWithRepository.FindPasswordAsync(cancellationToken);

            NUnit.Framework.Assert.That(result, Is.EqualTo(decryptedPassword));
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldEncryptPasswordAndWriteToFile()
        {
            string newPassword = "newPassword";
            string encryptedPassword = "encryptedNewPassword";
            CancellationToken cancellationToken = CancellationToken.None;

            _mockEncryption
                .Setup(e => e.EncryptStringAsync(newPassword, cancellationToken))
                .ReturnsAsync(encryptedPassword);

            await _workWithRepository.ChangePasswordAsync(newPassword, cancellationToken);

            _mockRepository.Verify(r => r.WriteToFileAsync(encryptedPassword, cancellationToken), Times.Once);
        }

        [Test]
        public void AddPasswordAsync_ShouldThrowIfCancelled()
        {
            string password = "myPassword";
            var cancellationToken = new CancellationToken(canceled: true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => _workWithRepository.AddPasswordAsync(password, cancellationToken));
        }

        [Test]
        public void FindPasswordAsync_ShouldThrowIfCancelled()
        {
            var cancellationToken = new CancellationToken(canceled: true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => _workWithRepository.FindPasswordAsync(cancellationToken));
        }

        [Test]
        public void ChangePasswordAsync_ShouldThrowIfCancelled()
        {
            string newPassword = "newPassword";
            var cancellationToken = new CancellationToken(canceled: true);

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(() => _workWithRepository.ChangePasswordAsync(newPassword, cancellationToken));
        }
    }
}
