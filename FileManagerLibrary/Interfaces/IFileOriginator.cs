namespace FileManagerLibrary.Interfaces {
  public interface IFileOriginator {
    object SaveCurrentStateToMemento();

    void RestoreStateFromMemento(object mementoObject);
  }
}
