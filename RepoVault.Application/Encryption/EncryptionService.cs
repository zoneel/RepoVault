﻿using System.Security.Cryptography;
using System.Text;

namespace RepoVault.Application.Encryption;

public class EncryptionService : IEncryptionService
{
    private const int KeySize = 256; 

    public async Task EncryptFolderAsync(string folderPath, string password)
    {
        try
        {
            byte[] key = GenerateKey(password);

            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

            foreach (string filePath in jsonFiles)
            {
                string jsonData = await File.ReadAllTextAsync(filePath);

                string encryptedData = EncryptStringAes(jsonData, key);

                string encryptedFilePath = filePath + ".encrypted";
                await File.WriteAllTextAsync(encryptedFilePath, encryptedData);

                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error encrypting folder: {ex.Message}");
        }
    }

    public async Task DecryptFolderAsync(string folderPath, string password)
    {
        try
        {
            byte[] key = GenerateKey(password);

            string[] encryptedJsonFiles = Directory.GetFiles(folderPath, "*.json.encrypted");

            foreach (string filePath in encryptedJsonFiles)
            {
                string encryptedData = await File.ReadAllTextAsync(filePath);

                string decryptedData = DecryptStringAes(encryptedData, key);

                string decryptedFilePath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(filePath));
                await File.WriteAllTextAsync(decryptedFilePath, decryptedData);

                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error decrypting folder: {ex.Message}");
        }
    }
 
    public byte[] GenerateKey(string password)
    {
        using var keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("salt"), 10000, HashAlgorithmName.SHA256);
        return keyDerivation.GetBytes(KeySize / 8);
    }

    public string EncryptStringAes(string plainText, byte[] password)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = password;
        aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
        }
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string DecryptStringAes(string cipherText, byte[] password)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = password;
        aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

        var decryption = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        using var csDecrypt = new CryptoStream(msDecrypt, decryption, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }

}