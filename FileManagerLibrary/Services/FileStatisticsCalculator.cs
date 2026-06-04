using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagerLibrary.Services {
  public class FileStatisticsCalculator {
    private readonly long _bytesPerKilobyte = 1024;
    private readonly int _separatorLineLength = 50;

    public virtual long CalculateTotalSizeOfFiles(List<FileInfo> filesToCalculate) {
      return filesToCalculate.Sum(file => file.Length);
    }

    public virtual string GetDetailedFolderStatistics(string directoryPath) {
      FileScanner directoryScanner = new FileScanner();
      List<FileInfo> allFiles = directoryScanner.ScanDirectoryForAllFiles(directoryPath);

      long totalSizeInBytes = CalculateTotalSizeOfFiles(allFiles);
      long totalSizeInKilobytes = totalSizeInBytes / _bytesPerKilobyte;
      int totalFileCount = allFiles.Count;

      Dictionary<string, int> filesGroupedByExtension = allFiles
        .GroupBy(file => file.Extension)
        .ToDictionary(group => group.Key, group => group.Count());

      string statisticsResult = $" СТАТИСТИКА ПАПКИ: {directoryPath}\n";
      statisticsResult += new string('═', _separatorLineLength) + "\n";
      statisticsResult += $" Всего файлов: {totalFileCount}\n";
      statisticsResult += $" Общий размер: {totalSizeInKilobytes:N0} КБ ({totalSizeInBytes:N0} байт)\n";
      statisticsResult += " Распределение по расширениям:\n";

      foreach (KeyValuePair<string, int> extensionGroup in filesGroupedByExtension) {
        string extensionDisplay;
        if (string.IsNullOrEmpty(extensionGroup.Key)) {
          extensionDisplay = "(без расширения)";
        } else {
          extensionDisplay = extensionGroup.Key;
        }

        statisticsResult += $"  {extensionDisplay}: {extensionGroup.Value} шт.\n";
      }

      return statisticsResult;
    }
  }
}