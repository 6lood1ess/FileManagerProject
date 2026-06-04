namespace FileManagerLibrary.Interfaces {
  public interface IEncryptionService {
    void EncryptFile(string inputFilePath, string outputFilePath);
    void DecryptFile(string inputFilePath, string outputFilePath);
    bool IsEncryptionKeyAvailable { get; }
  }
}
