namespace FileManagerConsole.Views {
  public class ConsoleView {
    public void DisplayMainMenu() {
      Console.Clear();

      Console.WriteLine(new string('═', 70) +
                        "\n            ФАЙЛОВЫЙ МЕНЕДЖЕР\n" +
                        new string('═', 70) +
                        "\n  1. Организация файлов\n" +
                        "  2. Шифрование файлов\n" +
                        "  3. Резервное копирование\n" +
                        "  4. Выход\n" +
                        new string('═', 70));

      Console.Write("\n  Выберите пункт: ");
    }

    public void DisplayOrganizationSubMenu() {
      Console.Clear();

      Console.WriteLine("-| ОРГАНИЗАЦИЯ ФАЙЛОВ |-\n" +
                        "  1. Отсортировать файлы по дате\n" +
                        "  2. Переместить файлы по расширению\n" +
                        "  3. Показать статистику папки\n" +
                        "  4. Сгруппировать файлы по типам\n" +
                        "  5. Назад\n");

      Console.Write("Выберите действие: ");
    }

    public void DisplayEncryptionSubMenu() {
      Console.Clear();

      Console.WriteLine("-| ШИФРОВАНИЕ ФАЙЛОВ |-\n" +
                        "  1. Инициализировать ключ шифрования\n" +
                        "  2. Зашифровать файл\n" +
                        "  3. Расшифровать файл\n" +
                        "  4. Назад\n");

      Console.Write("Выберите действие: ");
    }

    public void DisplayBackupSubMenu() {
      Console.Clear();

      Console.WriteLine("-| РЕЗЕРВНОЕ КОПИРОВАНИЕ |-\n" +
                        "  1. Создать бэкап файла\n" +
                        "  2. Восстановить файл\n" +
                        "  3. Показать историю бэкапов\n" +
                        "  4. Назад\n");

      Console.Write("Выберите действие: ");
    }

    public string GetUserInput(string prompt) {
      Console.Write(prompt);
      return Console.ReadLine() ?? string.Empty;
    }

    public void ShowSuccess(string message) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"{message}");
      Console.ResetColor();
    }

    public void ShowError(string message) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"{message}");
      Console.ResetColor();
    }

    public void ShowMessage(string message){
      Console.WriteLine(message);
    }

    public void WaitForKey() {
      Console.WriteLine("\nНажмите любую клавишу...");
      Console.ReadKey();
    }
  }
}
