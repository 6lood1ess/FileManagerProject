using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagerLibrary.Services {
  public class FileOrganizerFacade {
    private const int FileNameColumnWidth = 40;

    private readonly int _separatorLineLength = 60;
    private readonly int _displayLimit = 10;
    private readonly FileScanner _fileScannerSubsystem;
    private readonly FileMover _fileMoverSubsystem;
    private readonly FileStatisticsCalculator _statisticsCalculatorSubsystem;

    public FileOrganizerFacade(FileScanner fileScanner, FileMover fileMover, FileStatisticsCalculator statisticsCalculator) {
      _fileScannerSubsystem = fileScanner;
      _fileMoverSubsystem = fileMover;
      _statisticsCalculatorSubsystem = statisticsCalculator;
    }

    public void SortFilesByLastModifiedDate(string targetDirectoryPath, bool sortAscending = true) {
      List<FileInfo> allFiles = _fileScannerSubsystem.ScanDirectoryForAllFiles(targetDirectoryPath);

      List<FileInfo> sortedFiles;
      if (sortAscending) {
        sortedFiles = allFiles.OrderBy(file => file.LastWriteTime).ToList();
      } else {
        sortedFiles = allFiles.OrderByDescending(file => file.LastWriteTime).ToList();
      }

      Console.WriteLine($"\n Сортировка файлов в папке: {targetDirectoryPath}");
      Console.WriteLine(new string('─', _separatorLineLength));

      int filesToDisplay = Math.Min(_displayLimit, sortedFiles.Count);
      for (int fileIndex = 0; fileIndex < filesToDisplay; ++fileIndex) {
        FileInfo currentFile = sortedFiles[fileIndex];
        Console.WriteLine($"   {currentFile.Name,-FileNameColumnWidth} {currentFile.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
      }
    }

    public int MoveFilesByFileExtension(string sourceDirectoryPath, string targetFileExtension, string destinationDirectoryPath) {
      List<FileInfo> filesToMove = _fileScannerSubsystem.ScanDirectoryByFileExtension(sourceDirectoryPath, targetFileExtension);

      if (filesToMove.Count == 0) {
        Console.WriteLine($" Файлы с расширением {targetFileExtension} не найдены");
        return 0;
      }

      Console.WriteLine($"Найдено {filesToMove.Count} файлов с расширением {targetFileExtension}\nПеремещено {filesToMove.Count} файлов в {destinationDirectoryPath}");
      _fileMoverSubsystem.MoveFilesToDestination(filesToMove, destinationDirectoryPath);

      return filesToMove.Count;
    }

    public string GetFolderStatisticsReport(string targetDirectoryPath) {
      return _statisticsCalculatorSubsystem.GetDetailedFolderStatistics(targetDirectoryPath);
    }

    public Dictionary<string, int> GroupFilesByFileType(string sourceDirectoryPath) {
      List<FileInfo> allFiles = _fileScannerSubsystem.ScanDirectoryForAllFiles(sourceDirectoryPath);

      IEnumerable<IGrouping<string, FileInfo>> filesGroupedByExtension = allFiles.GroupBy(file => file.Extension.ToLower());
      Dictionary<string, int> groupingResult = new Dictionary<string, int>();

      foreach (IGrouping<string, FileInfo> extensionGroup in filesGroupedByExtension) {
        string groupFolderName = extensionGroup.Key.TrimStart('.');
        if (string.IsNullOrEmpty(groupFolderName)) {
          groupFolderName = "files_without_extension";
        }

        string destinationGroupFolder = Path.Combine(sourceDirectoryPath, groupFolderName);
        List<FileInfo> filesInThisGroup = extensionGroup.ToList();

        _fileMoverSubsystem.MoveFilesToDestination(filesInThisGroup, destinationGroupFolder);
        groupingResult.Add(extensionGroup.Key, filesInThisGroup.Count);
      }

      Console.WriteLine($" Файлы в {sourceDirectoryPath} сгруппированы по типам");
      return groupingResult;
    }
  }
}