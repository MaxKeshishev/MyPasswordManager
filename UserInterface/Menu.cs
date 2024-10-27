using Logic;
using Storage;

namespace UserInterface
{
    public class Menu
    {
        private Encryption encryption;

        private async Task ShowMenuAsync(Dictionary<int, Func<Task>> options, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine("> Введите цифру:");
            foreach (var option in options)

                if (int.TryParse(Console.ReadLine(), out int choice) && options.ContainsKey(choice))
                {
                    await options[choice].Invoke();
                    return;
                }
                else
                {
                    Console.WriteLine("> Неверный формат ввода");
                }
        }

        public async Task StartMenuAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Dictionary<int, Func<Task>> menuOptions = new Dictionary<int, Func<Task>>
            {
                { 1, () => OptionLoginAsync(cancellationToken) },
                { 2, () => OptionSignUpAsync(cancellationToken) },
                { 3, () => OptionExit(cancellationToken) }
            };

            Console.WriteLine();
            Console.WriteLine("> Выберите действие:");
            Console.WriteLine("1. Вход");
            Console.WriteLine("2. Регистрация");
            Console.WriteLine("3. Выход");

            await ShowMenuAsync(menuOptions, cancellationToken);
        }

        private async Task MainMenuAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Dictionary<int, Func<Task>> menuOptions = new Dictionary<int, Func<Task>>
            {
                { 1, () => OptionAddPasswordAsync(cancellationToken) },
                { 2, () => OptionFindPasswordAsync(cancellationToken) },
                { 3, () => OptionChangePasswordAsync(cancellationToken) },
                { 4, () => OptionExit(cancellationToken) }
            };

            Console.WriteLine();
            Console.WriteLine("> Выберите действие:");
            Console.WriteLine("1. Добавить новый пароль");
            Console.WriteLine("2. Поиск пароля");
            Console.WriteLine("3. Изменить пароль");
            Console.WriteLine("4. Выход");

            await ShowMenuAsync(menuOptions, cancellationToken);
        }

        private async Task OptionSignUpAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string login = CheckInput("> Введите логин:", cancellationToken);
            string masterPassword = CheckInput("> Введите мастер-пароль:", cancellationToken);

            var autorisation = new Autorisation(login, masterPassword);
            encryption = await autorisation.SignUpAsync(cancellationToken);

            Console.WriteLine("> Добро пожаловать в менеджер паролей!");
            await MainMenuAsync(cancellationToken);
        }

        private async Task OptionLoginAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string login = CheckInput("> Введите логин:", cancellationToken);
                string masterPassword = CheckInput("> Введите мастер-пароль:", cancellationToken);

                var autorisation = new Autorisation(login, masterPassword);
                encryption = await autorisation.LoginAsync(cancellationToken);

                if (encryption != null)
                {
                    Console.WriteLine("> Добро пожаловать в менеджер паролей!");
                    await MainMenuAsync(cancellationToken);
                }
                else
                {
                    Console.WriteLine("> Пароль неверный");
                    await StartMenuAsync(cancellationToken);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("> Пользователя с таким логином не существует");
                await StartMenuAsync(cancellationToken);
            }
        }

        private async Task OptionAddPasswordAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string login = CheckInput("> Введите логин:", cancellationToken);
            string password = CheckInput("> Введите пароль:", cancellationToken);

            var workWithRepository = new WorkWithRepository(new Repository(login), encryption);

            await workWithRepository.AddPasswordAsync(password, cancellationToken);

            Console.WriteLine("> Пароль успешно добавлен!");

            await MainMenuAsync(cancellationToken);
        }

        private async Task OptionFindPasswordAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string login = CheckInput("> Введите логин:", cancellationToken);

                var workWithRepository = new WorkWithRepository(new Repository(login), encryption);

                var result = await workWithRepository.FindPasswordAsync(cancellationToken);

                if (result == null)
                    Console.WriteLine("> Данный логин принадлежит другому пользователю");
                else
                    Console.WriteLine(result);

                await MainMenuAsync(cancellationToken);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("> Такого логина не существует");
                await MainMenuAsync(cancellationToken);
            }
        }

        private async Task OptionChangePasswordAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string login = CheckInput("> Введите логин:", cancellationToken);
                string newPassword = CheckInput("> Введите новый пароль:", cancellationToken);

                var workWithRepository = new WorkWithRepository(new Repository(login), encryption);

                string check = await workWithRepository.FindPasswordAsync(cancellationToken);

                if (check == null)
                    Console.WriteLine("> Данный логин принадлежит другому пользователю");
                else
                {
                    await workWithRepository.ChangePasswordAsync(newPassword, cancellationToken);
                    Console.WriteLine("> Пароль успешно изменён!");
                }

                await MainMenuAsync(cancellationToken);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("> Такого логина не существует");
                await MainMenuAsync(cancellationToken);
            }
        }

        private Task OptionExit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine("> Выход из программы...");
            return Task.CompletedTask;
        }

        private string CheckInput(string print, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string result = string.Empty;
            while (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine(print);
                result = Console.ReadLine();

                if (result.All(c => char.IsLetterOrDigit(c) && c < 128) == false)
                {
                    Console.WriteLine("> Данные содержат недопустимые символы");
                    result = string.Empty;
                }

                if (result.Length > 16)
                {
                    Console.WriteLine("> Длина строки не должна превышать 16 символов");
                    result = string.Empty;
                }
            }
            return result;
        }
    }
}
