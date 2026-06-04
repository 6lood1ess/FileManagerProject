using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileManagerLibrary.Services;

namespace FileManagerTests {
  [TestClass]
  public class FacadeTests {
    private string _testDirectory;

    [TestInitialize]
    public void Setup() {
      _testDirectory = Path.Combine(Path.GetTempPath(), "FacadeTest_" + Guid.NewGuid().ToString());
      Directory.CreateDirectory(_testDirectory);
      File.WriteAllText(Path.Combine(_testDirectory, "file1.txt"), "content1");
      File.WriteAllText(Path.Combine(_testDirectory, "file2.txt"), "content2");
      File.WriteAllText(Path.Combine(_testDirectory, "image.jpg"), "jpeg");
    }

    [TestCleanup]
    public void Cleanup() {
      Directory.Delete(_testDirectory, true);
    }

    [TestMethod]
    [TestCategory("Facade")]
    public void FileScanner_ScanDirectoryForAllFiles_ShouldReturnAllFiles() {
      FileScanner scanner = new FileScanner();
      List<FileInfo> files = scanner.ScanDirectoryForAllFiles(_testDirectory);
      Assert.AreEqual(3, files.Count);
    }

    [TestMethod]
    [TestCategory("Facade")]
    public void FileScanner_ScanDirectoryByFileExtension_ShouldFilterCorrectly() {
      FileScanner scanner = new FileScanner();
      List<FileInfo> txtFiles = scanner.ScanDirectoryByFileExtension(_testDirectory, ".txt");
      Assert.AreEqual(2, txtFiles.Count);
    }

    [TestMethod]
    [TestCategory("Facade")]
    public void FileOrganizerFacade_WithMocks_ShouldCallSubsystems() {
      Mock<FileScanner> mockScanner = new Mock<FileScanner>();
      Mock<FileMover> mockMover = new Mock<FileMover>();
      Mock<FileStatisticsCalculator> mockStats = new Mock<FileStatisticsCalculator>();

      mockScanner.Setup(s => s.ScanDirectoryForAllFiles(It.IsAny<string>()))
        .Returns(new List<FileInfo>());

      FileOrganizerFacade facade = new FileOrganizerFacade(mockScanner.Object, mockMover.Object, mockStats.Object);
      facade.GroupFilesByFileType(_testDirectory);

      mockScanner.Verify(s => s.ScanDirectoryForAllFiles(_testDirectory), Times.Once);
      mockMover.Verify(m => m.MoveFilesToDestination(It.IsAny<List<FileInfo>>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [DataTestMethod]
    [DataRow(".txt", 2)]
    [DataRow(".jpg", 1)]
    [DataRow(".pdf", 0)]
    [TestCategory("Facade")]
    public void MoveFilesByFileExtension_ShouldReturnCorrectCount(string extension, int expectedCount) {
      FileOrganizerFacade facade = new FileOrganizerFacade(new FileScanner(), new FileMover(), new FileStatisticsCalculator());
      string destDir = Path.Combine(_testDirectory, "destination");
      int movedCount = facade.MoveFilesByFileExtension(_testDirectory, extension, destDir);
      Assert.AreEqual(expectedCount, movedCount);
    }
  }
}