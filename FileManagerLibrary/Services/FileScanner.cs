using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagerLibrary.Services {
  public class FileScanner {
    public virtual List<FileInfo> ScanDirectoryForAllFiles(string directoryPath) {
      if (!Directory.Exists(directoryPath)) { 
        throw new DirectoryNotFoundException($"Папка {directoryPath} не найдена");
      }
      
      DirectoryInfo directoryInformation = new DirectoryInfo(directoryPath);
      return directoryInformation.GetFiles().ToList();
    }
        
    public virtual List<FileInfo> ScanDirectoryByFileExtension(string directoryPath, string fileExtension) {
      List<FileInfo> allFilesInDirectory = ScanDirectoryForAllFiles(directoryPath);
      return allFilesInDirectory
        .Where(file => file.Extension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
        .ToList();
    }
  }
}