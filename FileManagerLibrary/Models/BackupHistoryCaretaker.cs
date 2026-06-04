using FileManagerLibrary.Interfaces;

namespace FileManagerLibrary.Models {
  public class BackupHistoryCaretaker {
    private Stack<object> _backupHistoryStack = new Stack<object>();

    public void SaveCurrentStateToHistory(IFileOriginator originatorToBackup) {
      object savedMemento = originatorToBackup.SaveCurrentStateToMemento();
      _backupHistoryStack.Push(savedMemento);
      Console.WriteLine($"Состояние сохранено в Caretaker. Всего бэкапов: {_backupHistoryStack.Count}");
    }

    public void RestoreLastStateFromHistory(IFileOriginator originatorToRestore) {
      if (_backupHistoryStack.Count > 0) {
        object lastMemento = _backupHistoryStack.Pop();
        originatorToRestore.RestoreStateFromMemento(lastMemento);
        Console.WriteLine($"Состояние восстановлено. Осталось бэкапов: {_backupHistoryStack.Count}");
      } else {
        Console.WriteLine("Нет сохранённых бэкапов в истории");
      }
    }

    public bool HasBackupHistory() {
      return _backupHistoryStack.Count > 0;
    }

    public int GetBackupHistoryCount() {
      return _backupHistoryStack.Count;
    }

    public void ClearBackupHistory() {
      _backupHistoryStack.Clear();
    }
  }
}
