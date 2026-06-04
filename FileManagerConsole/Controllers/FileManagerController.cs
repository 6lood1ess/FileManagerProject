using FileManagerLibrary.Models;
using FileManagerLibrary.Services;
using FileManagerConsole.Views;

namespace FileManagerConsole.Controllers {
  public class FileManagerController {
    private ConsoleView _userInterface;

    private EncryptionKeyManager _encryptionKeySingletonManager;
    private FileOrganizerFacade _fileOrganizationFacade;
    private BackupHistoryCaretaker _backupHistoryCaretaker;
    private FileOriginatorForBackup _currentFileOriginator;

    public FileManagerController() {
      _userInterface = new ConsoleView();

      _encryptionKeySingletonManager = EncryptionKeyManager.Instance;

      FileScanner fileScannerSubsystem = new FileScanner();
      FileMover fileMoverSubsystem = new FileMover();
      FileStatisticsCalculator statisticsCalculatorSubsystem = new FileStatisticsCalculator();
      _fileOrganizationFacade = new FileOrganizerFacade(fileScannerSubsystem, fileMoverSubsystem, statisticsCalculatorSubsystem);

      _backupHistoryCaretaker = new BackupHistoryCaretaker();
    }

    public void Run() {
      bool isRunning = true;

      while (isRunning) {
        _userInterface.DisplayMainMenu();
        string choice = Console.ReadLine();

        switch (choice) {
          case "1":
            RunOrganizationModule();
            break;
          case "2":
            RunEncryptionModule();
            break;
          case "3":
            RunBackupModule();
            break;
          case "4":
            isRunning = false;
            _userInterface.ShowSuccess("До свидания!");
            break;
          default:
            _userInterface.ShowError("Неверный выбор");
            _userInterface.WaitForKey();
            break;
        }
      }
    }

    private void RunOrganizationModule() {
      bool back = false;
      
      while (!back) {
        _userInterface.DisplayOrganizationSubMenu();
        string choice = Console.ReadLine();

        switch (choice) {
          case "1":
            string dir = _userInterface.GetUserInput("Введите путь к папке: ");

            if (Directory.Exists(dir)) {
                            _fileOrganizationFacade.SortFilesByLastModifiedDate(dir);
            } else {
              _userInterface.ShowError("Папка не найдена");
            }

            _userInterface.WaitForKey();
            break;
          case "2":
            string source = _userInterface.GetUserInput("Исходная папка: ");
            string ext = _userInterface.GetUserInput("Расширение (например .txt): ");
            string dest = _userInterface.GetUserInput("Папка назначения: ");

            if (Directory.Exists(source)) {
              int count = _fileOrganizationFacade.MoveFilesByFileExtension(source, ext, dest);
              _userInterface.ShowSuccess($"Перемещено {count} файлов");
            }

            _userInterface.WaitForKey();
            break;
          case "3":
            string statsDir = _userInterface.GetUserInput("Путь к папке: ");

            if (Directory.Exists(statsDir)) {
              _userInterface.ShowMessage(_fileOrganizationFacade.GetFolderStatisticsReport(statsDir));
            }

            _userInterface.WaitForKey();
            break;
          case "4":
            string groupDir = _userInterface.GetUserInput("Путь к папке: ");

            if (Directory.Exists(groupDir)) {
              _fileOrganizationFacade.GroupFilesByFileType(groupDir);
            }

            _userInterface.WaitForKey();
            break;
          case "5":
            back = true;
            break;
          default:
            _userInterface.ShowError("Неверный выбор");
            _userInterface.WaitForKey();
            break;
        }
      }
    }

    private void RunEncryptionModule() {
      bool back = false;

      while (!back) {
        _userInterface.DisplayEncryptionSubMenu();
        string choice = Console.ReadLine();

        switch (choice) {
          case "1":
            _encryptionKeySingletonManager.InitializeEncryptionKey();
            _userInterface.ShowSuccess("Ключ шифрования инициализирован (Singleton)");
            _encryptionKeySingletonManager.DisplaySingletonInformation();
            
            _userInterface.WaitForKey();
            break;
          case "2":
            string file = _userInterface.GetUserInput("Файл для шифрования: ");

            if (File.Exists(file)) {
              _userInterface.ShowMessage($"Шифрование файла: {file}");
            } else {
              _userInterface.ShowError("Файл не найден");
            }

            _userInterface.WaitForKey();
            break;
          case "3":
            _userInterface.ShowMessage("Расшифрование файлов будет реализовано");
            _userInterface.WaitForKey();
            break;
          case "4":
            back = true;
            break;
          default:
            _userInterface.ShowError("Неверный выбор");
            _userInterface.WaitForKey();
            break;
        }
      }
    }

    private void RunBackupModule() {
      bool back = false;

      while (!back) {
        _userInterface.DisplayBackupSubMenu();
        string choice = Console.ReadLine();

        switch (choice) {
          case "1":
            string file = _userInterface.GetUserInput("Путь к файлу для бэкапа: ");
          
            if (File.Exists(file)) {
              _currentFileOriginator = new FileOriginatorForBackup(file);
              _backupHistoryCaretaker.SaveCurrentStateToHistory(_currentFileOriginator);
              _userInterface.ShowSuccess("Бэкап создан (Memento сохранён в Caretaker)");
            } else {
              _userInterface.ShowError("Файл не найден");
            }

            _userInterface.WaitForKey();
            break;
          case "2":
            if (_currentFileOriginator != null && _backupHistoryCaretaker.HasBackupHistory()) {
              _backupHistoryCaretaker.RestoreLastStateFromHistory(_currentFileOriginator);
              _userInterface.ShowSuccess("Файл восстановлен из бэкапа");
            } else {
              _userInterface.ShowError("Нет сохранённых бэкапов");
            }

            _userInterface.WaitForKey();
            break;
          case "3":
            _userInterface.ShowMessage($"Количество бэкапов в истории: {_backupHistoryCaretaker.GetBackupHistoryCount()}");
            _userInterface.WaitForKey();
            break;
          case "4":
            back = true;
            break;
          default:
            _userInterface.ShowError("Неверный выбор");
            _userInterface.WaitForKey();
            break;
        }
      }
    }
  }
}
