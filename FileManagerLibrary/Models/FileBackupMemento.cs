using System.Security.Cryptography;
using System.Text;

namespace FileManagerLibrary.Models {
  public class FileBackupMemento {
    public string OriginalFilePath;
    public byte[] FileContentBackup;
    public DateTime BackupCreationTime;
    public long FileSizeInBytes;
    public string FileNameWithoutPath;
    public string FileContentHash;

    public FileBackupMemento(string filePath, byte[] fileContent) {
      OriginalFilePath = filePath;
      FileContentBackup = fileContent;
      BackupCreationTime = DateTime.Now;
      FileSizeInBytes = fileContent.Length;
      FileNameWithoutPath = System.IO.Path.GetFileName(filePath);
      FileContentHash = ComputeSha256Hash(fileContent);
    }

    public void DisplayMementoInformation() {
      Console.WriteLine($"Memento: {FileNameWithoutPath}, размер = {FileSizeInBytes} байт, хеш = {FileContentHash}");
    }

    private string ComputeSha256Hash(byte[] contentToHash) {
      using (SHA256 sha256HashAlgorithm = SHA256.Create()) {
        byte[] hashBytes = sha256HashAlgorithm.ComputeHash(contentToHash);

        int maximumHashLengthForDisplay = 8;
        int currentBytePosition = 0;
        int upperBoundary = Math.Min(maximumHashLengthForDisplay, hashBytes.Length);

        StringBuilder stringBuilder = new StringBuilder();

        for (currentBytePosition = 0; currentBytePosition < upperBoundary; ++currentBytePosition) {
          stringBuilder.Append(hashBytes[currentBytePosition].ToString("X2"));
        }

        return stringBuilder.ToString();
      }
    }
  }
}
