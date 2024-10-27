using UserInterface;

namespace PasswordManager
{
    internal class Manager
    {
        static async Task Main(string[] args)
        {
            var manager = new Menu();
          
            await manager.StartMenuAsync(new CancellationToken(false));
        }
    }
}
