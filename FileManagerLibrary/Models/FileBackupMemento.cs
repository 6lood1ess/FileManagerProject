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
      if (contentToHash == null || contentToHash.Length == 0) {
        return "EMPTY_HASH";
      }

      using (SHA256 sha256HashAlgorithm = SHA256.Create()) {
        byte[] computedHashBytes = sha256HashAlgorithm.ComputeHash(contentToHash);

                return Convert.ToHexString(computedHashBytes);
            }
    }
  }
}
