using System;
using System.Security.Cryptography;

namespace FileManagerLibrary.Models {
  public sealed class EncryptionKeyManager {
    private static readonly object _threadLockObject = new object();
    private static EncryptionKeyManager _uniqueInstance;
   
    private byte[] _encryptionKey;
    private byte[] _initializationVector;
    private bool _isKeyInitialized = false;

    public static EncryptionKeyManager Instance {
      get {
        if (_uniqueInstance == null) {
          lock (_threadLockObject) {
            if (_uniqueInstance == null) {
              _uniqueInstance = new EncryptionKeyManager();
            }
          }
        }

        return _uniqueInstance;
      }
    }

    private EncryptionKeyManager() { }

    public bool IsEncryptionKeyInitialize {
      get {
        return _isKeyInitialized;
      }
    }

    public void InitializeEncryptionKey() {
      if (!_isKeyInitialized) {
        using (Aes aesEncryptionAlgorithm = Aes.Create()) {
          aesEncryptionAlgorithm.KeySize = 256;
          aesEncryptionAlgorithm.GenerateKey();
          aesEncryptionAlgorithm.GenerateIV();
          _encryptionKey = aesEncryptionAlgorithm.Key;
          _initializationVector = aesEncryptionAlgorithm.IV;
          _isKeyInitialized = true;
        }
      }
    }

    public (byte[] EncryptionKey, byte[] InitializationVector) GetEncryptionKeyAndVector() {
      if (!_isKeyInitialized) {
        throw new InvalidOperationException("Ключ шифрования не инициализирован");
      }   

      return (_encryptionKey, _initializationVector);
    }

    public void DisplaySingletonInformation() {
      Console.WriteLine($"Singleton: EncryptionKeyManager готов, ключ = {_encryptionKey?.Length ?? 0} байт");
    }
  }
}
