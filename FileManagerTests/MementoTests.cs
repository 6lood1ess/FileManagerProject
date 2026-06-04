using FileManagerLibrary.Models;
using System.Text;

namespace FileManagerTests {
  [TestClass]
  public class MementoTests {
    private string? _testFilePath;
    private byte[]? _originalContent;
    private byte[]? _modifiedContent;

    [TestInitialize]
    public void Setup() {
      _testFilePath = Path.GetTempFileName();
      _originalContent = Encoding.UTF8.GetBytes("Оригинальное содержимое файла");
      _modifiedContent = Encoding.UTF8.GetBytes("Изменённое содержимое файла");
      File.WriteAllBytes(_testFilePath, _originalContent);
    }

    [TestCleanup]
    public void Cleanup() {
      if (_testFilePath != null && File.Exists(_testFilePath)) {
        File.Delete(_testFilePath);
      }
    }

    [TestMethod]
    [TestCategory("Memento")]
    [Description("Проверяет, что FileOriginator корректно сохраняет состояние в Memento " + 
                 "и восстанавливает из него")]
    public void FileOriginator_SaveAndRestoreState_ShouldPreserveOriginalContent() {
      Assert.IsNotNull(_testFilePath, "_testFilePath не должен быть null");
      Assert.IsNotNull(_originalContent, "_originalContent не должен быть null");
      Assert.IsNotNull(_modifiedContent, "_modifiedContent не должен быть null");
            
      FileOriginatorForBackup originator = new FileOriginatorForBackup(_testFilePath);
      object savedMemento = originator.SaveCurrentStateToMemento();
            
      originator.ModifyFileWithNewContent(_modifiedContent);
      byte[] contentAfterModification = File.ReadAllBytes(_testFilePath);
      CollectionAssert.AreNotEqual(_originalContent, contentAfterModification, 
                                   "Файл должен был измениться после вызова ModifyFileWithNewContent");
            
      originator.RestoreStateFromMemento(savedMemento);
      byte[] contentAfterRestoration = File.ReadAllBytes(_testFilePath);
      CollectionAssert.AreEqual(_originalContent, contentAfterRestoration, 
                                "После восстановления из Memento файл должен вернуться к исходному состоянию");
    }

    [TestMethod]
    [TestCategory("Memento")]
    [Description("Проверяет, что BackupCaretaker корректно сохраняет " + 
                 "и восстанавливает несколько состояний (стек LIFO)")]
    public void BackupHistoryCaretaker_SaveAndRestore_ShouldWorkInLifoOrder() {
      Assert.IsNotNull(_testFilePath, "_testFilePath не должен быть null");
      Assert.IsNotNull(_originalContent, "_originalContent не должен быть null");
      Assert.IsNotNull(_modifiedContent, "_modifiedContent не должен быть null");
      
      FileOriginatorForBackup originator = new FileOriginatorForBackup(_testFilePath);
      BackupHistoryCaretaker caretaker = new BackupHistoryCaretaker();
      
      byte[] thirdVersionContent = Encoding.UTF8.GetBytes("Версия 3: третий бэкап финальная версия");
      
      File.WriteAllBytes(_testFilePath, _originalContent);
      originator = new FileOriginatorForBackup(_testFilePath);
      caretaker.SaveCurrentStateToHistory(originator);
      
      File.WriteAllBytes(_testFilePath, _modifiedContent);
      originator = new FileOriginatorForBackup(_testFilePath);
      caretaker.SaveCurrentStateToHistory(originator);
      
      File.WriteAllBytes(_testFilePath, thirdVersionContent);
      originator = new FileOriginatorForBackup(_testFilePath);
      caretaker.SaveCurrentStateToHistory(originator);
      
      Assert.AreEqual(3, caretaker.GetBackupHistoryCount(), 
                      "Должно быть сохранено 3 бэкапа в истории");
            
      caretaker.RestoreLastStateFromHistory(originator);
      byte[] contentAfterFirstRestore = File.ReadAllBytes(_testFilePath);
      CollectionAssert.AreEqual(_modifiedContent, contentAfterFirstRestore, 
                                "После первого восстановления должен быть файл второй версии");
            
      caretaker.RestoreLastStateFromHistory(originator);
      byte[] contentAfterSecondRestore = File.ReadAllBytes(_testFilePath);
      CollectionAssert.AreEqual(_originalContent, contentAfterSecondRestore, 
                                "После второго восстановления должен быть файл первой версии");
            
      Assert.AreEqual(1, caretaker.GetBackupHistoryCount(), 
                      "После двух восстановлений в истории должен остаться 1 бэкап");
    }

    [TestMethod]
    [TestCategory("Memento")]
    [Description("Проверяет, что Memento хранит корректные метаданные о файле")]
    public void FileBackupMemento_ShouldStoreCorrectMetadata() {
      Assert.IsNotNull(_testFilePath, "_testFilePath не должен быть null");
      Assert.IsNotNull(_originalContent, "_originalContent не должен быть null");

      FileBackupMemento backupMemento = new FileBackupMemento(_testFilePath, _originalContent);

      Assert.AreEqual(_testFilePath, backupMemento.OriginalFilePath,
                      "Memento должен хранить правильный путь к файлу");
      Assert.AreEqual(_originalContent.Length, backupMemento.FileSizeInBytes,
                      "Memento должен хранить правильный размер файла");
      Assert.AreEqual(Path.GetFileName(_testFilePath), backupMemento.FileNameWithoutPath,
                      "Memento должен хранить правильное имя файла");
      Assert.IsNotNull(backupMemento.FileContentHash,
                       "Memento должен вычислять хеш содержимого");
      Assert.IsGreaterThan(backupMemento.FileContentHash.Length, 0,
                    "Хеш не должен быть пустым");
      Assert.IsTrue(backupMemento.BackupCreationTime <= DateTime.Now,
                    "Время создания бэкапа должно быть в прошлом или настоящем");
    }

    [TestMethod]
    [TestCategory("Memento")]
    [Description("Проверяет, что Caretaker корректно сообщает о наличии истории")]
    public void BackupHistoryCaretaker_HasBackupHistory_ShouldReturnCorrectBoolean() {
      Assert.IsNotNull(_testFilePath, "_testFilePath не должен быть null");
      Assert.IsNotNull(_originalContent, "_originalContent не должен быть null");

      FileOriginatorForBackup originator = new FileOriginatorForBackup(_testFilePath);
      BackupHistoryCaretaker caretaker = new BackupHistoryCaretaker();

      Assert.IsFalse(caretaker.HasBackupHistory(),
                     "До сохранения бэкапов HasBackupHistory должен возвращать false");
      Assert.AreEqual(0, caretaker.GetBackupHistoryCount(),
                      "Количество бэкапов до сохранения должно быть 0");

      caretaker.SaveCurrentStateToHistory(originator);

      Assert.IsTrue(caretaker.HasBackupHistory(),
                    "После сохранения бэкапа HasBackupHistory должен возвращать true");
      Assert.AreEqual(1, caretaker.GetBackupHistoryCount(),
                      "Количество бэкапов после сохранения должно быть 1");

      caretaker.ClearBackupHistory();

      Assert.IsFalse(caretaker.HasBackupHistory(),
                     "После очистки истории HasBackupHistory должен возвращать false");
      Assert.AreEqual(0, caretaker.GetBackupHistoryCount(),
                      "После очистки количество бэкапов должно быть 0");
    }
  }
}