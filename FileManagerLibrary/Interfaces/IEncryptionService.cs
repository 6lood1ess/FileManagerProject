namespace FileManagerLibrary.Interfaces {
  public interface IEncryptionService {
    bool IsEncryptionKeyAvailable { get; }

    void EncryptFile(string inputFilePath, string outputFilePath);

    void DecryptFile(string inputFilePath, string outputFilePath);
  }
}
