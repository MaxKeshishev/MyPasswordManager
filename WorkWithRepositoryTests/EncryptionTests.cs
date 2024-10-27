using NUnit.Framework;

namespace Logic.Tests
{
    [TestFixture]
    public class EncryptionTests
    {
        private Encryption _encryption;

        [SetUp]
        public void Setup()
        {
            _encryption = new Encryption("testKey");
        }

        [Test]
        public async Task EncryptStringAsync_ShouldReturnEncryptedString_WhenCalledWithValidInput()
        {
            string plainText = "Hello, World!";
            string expectedEncryptedString;

            var encryptedString = await _encryption.EncryptStringAsync(plainText, CancellationToken.None);

            NUnit.Framework.Assert.IsNotNull(encryptedString);
            NUnit.Framework.Assert.That(encryptedString, Is.Not.EqualTo(plainText));
            expectedEncryptedString = encryptedString;
        }

        [Test]
        public async Task DecryptStringAsync_ShouldReturnDecryptedString_WhenCalledWithEncryptedInput()
        {
            string plainText = "Hello, World!";
            var encryptedString = await _encryption.EncryptStringAsync(plainText, CancellationToken.None);

            var decryptedString = await _encryption.DecryptStringAsync(encryptedString, CancellationToken.None);

            NUnit.Framework.Assert.IsNotNull(decryptedString);
            NUnit.Framework.Assert.That(decryptedString, Is.EqualTo(plainText));
        }

        [Test]
        public async Task DecryptStringAsync_ShouldReturnNull_WhenCalledWithInvalidEncryptedInput()
        {
            string invalidCipherText = "InvalidCipherText";

            var result = await _encryption.DecryptStringAsync(invalidCipherText, CancellationToken.None);

            NUnit.Framework.Assert.IsNull(result);
        }

        [Test]
        public void EncryptStringAsync_ShouldThrowIfCancelled()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _encryption.EncryptStringAsync("Some text", cancellationTokenSource.Token));
        }

        [Test]
        public void DecryptStringAsync_ShouldThrowIfCancelled()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            NUnit.Framework.Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _encryption.DecryptStringAsync("Some encrypted text", cancellationTokenSource.Token));
        }
    }
}
