using System;
using System.Collections.Generic;
using System.IO;

namespace FileManagerLibrary.Services {
  public class FileMover {
    public virtual void MoveFilesToDestination(List<FileInfo> filesToMove, string destinationDirectoryPath) {
      if (!Directory.Exists(destinationDirectoryPath)) { 
        Directory.CreateDirectory(destinationDirectoryPath);
      }
      
      foreach (FileInfo currentFile in filesToMove) {
        string destinationFilePath = Path.Combine(destinationDirectoryPath, currentFile.Name);
        if (File.Exists(destinationFilePath)) { 
          File.Delete(destinationFilePath);
        }

        File.Move(currentFile.FullName, destinationFilePath);
      }
    }
    
    public virtual void CopyFilesToDestination(List<FileInfo> filesToCopy, string destinationDirectoryPath) {
      if (!Directory.Exists(destinationDirectoryPath)) { 
        Directory.CreateDirectory(destinationDirectoryPath);
      }

      foreach (FileInfo currentFile in filesToCopy) {
        string destinationFilePath = Path.Combine(destinationDirectoryPath, currentFile.Name);
        File.Copy(currentFile.FullName, destinationFilePath, overwrite: true);
      }
    }
  }
}