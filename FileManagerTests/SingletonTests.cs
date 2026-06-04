using FileManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagerTests {
  [TestClass]
  public class SingletonTests {
    [TestMethod]
    [TestCategory("Singleton")]
    public void EncryptionKeyManager_ShouldReturnSameInstance() {
      EncryptionKeyManager firstInstance = EncryptionKeyManager.Instance;
      EncryptionKeyManager secondInstance = EncryptionKeyManager.Instance;
      Assert.AreSame(firstInstance, secondInstance);
    }

    [TestMethod]
    [TestCategory("Singleton")]
    public void EncryptionKeyManager_ShouldGenerateKeyAfterInitialization() {
      EncryptionKeyManager manager = EncryptionKeyManager.Instance;
      manager.InitializeEncryptionKey();
      (byte[] encryptionKey, byte[] initializationVector) = manager.GetEncryptionKeyAndVector();

      Assert.IsNotNull(encryptionKey);
      Assert.AreEqual(32, encryptionKey.Length);
      Assert.IsNotNull(initializationVector);
      Assert.AreEqual(16, initializationVector.Length);
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]

    [TestCategory("Singleton")]
    public void EncryptionKeyManager_MultipleCalls_ShouldReturnSameInstance(int callCount) {
      EncryptionKeyManager[] instances = new EncryptionKeyManager[callCount];
      for (int callNumber = 0; callNumber < callCount; callNumber++) {
        instances[callNumber] = EncryptionKeyManager.Instance;
      }
             
      for (int callNumber = 1; callNumber < callCount; callNumber++) { 
        Assert.AreSame(instances[0], instances[callNumber]);
      }        
    }
  }
}