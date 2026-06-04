using FileManagerLibrary.Interfaces;

namespace FileManagerLibrary.Models {
  public class FileOriginatorForBackup : IFileOriginator {
    public string TargetFilePath;
    public byte[] CurrentFileContent;
        
    public FileOriginatorForBackup(string filePathToManage) {
      TargetFilePath = filePathToManage;
      CurrentFileContent = Array.Empty<byte>();

      LoadCurrentFileContentFromDisk();
    }
        
    public object SaveCurrentStateToMemento() {
      FileBackupMemento newBackup = new FileBackupMemento(TargetFilePath, CurrentFileContent);
      Console.WriteLine($"Состояние файла {Path.GetFileName(TargetFilePath)} сохранено в Memento");
      return newBackup;
    }
        
    public void RestoreStateFromMemento(object mementoObject) {
      if (mementoObject is FileBackupMemento backupMemento) {
        File.WriteAllBytes(backupMemento.OriginalFilePath, backupMemento.FileContentBackup);
        CurrentFileContent = backupMemento.FileContentBackup;
        Console.WriteLine($"Файл {backupMemento.FileNameWithoutPath} восстановлен из бэкапа");
      }
    }
        
    public void ModifyFileWithNewContent(byte[] newFileContent) {
      File.WriteAllBytes(TargetFilePath, newFileContent);
      CurrentFileContent = newFileContent;
      Console.WriteLine($"Файл {Path.GetFileName(TargetFilePath)} изменён");
    }

    private void LoadCurrentFileContentFromDisk() {
      if (File.Exists(TargetFilePath)) { 
        CurrentFileContent = File.ReadAllBytes(TargetFilePath);
      } else {
        CurrentFileContent = Array.Empty<byte>();
      }
    }
  }
}
