using System.Security.Cryptography;
using System.Text;

namespace RepoVault.Application.Encryption;

public class EncryptionService : IEncryptionService
{
    private const int KeySize = 256;

    #region Methods

    public async Task EncryptFolder(string folderPath, string password)
    {
        try
        {
            var key = GenerateKey(password);

            var jsonFiles = Directory.GetFiles(folderPath, "*.json");

            foreach (var filePath in jsonFiles)
            {
                var jsonData = File.ReadAllText(filePath);

                var encryptedData = EncryptStringAES(jsonData, key);

                var encryptedFilePath = filePath + ".encrypted";
                File.WriteAllText(encryptedFilePath, encryptedData);

                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error encrypting folder: {ex.Message}");
        }
    }

    public async Task DecryptFolder(string folderPath, string password)
    {
        try
        {
            var key = GenerateKey(password);

            var encryptedJsonFiles = Directory.GetFiles(folderPath, "*.json.encrypted");

            foreach (var filePath in encryptedJsonFiles)
            {
                var encryptedData = File.ReadAllText(filePath);

                var decryptedData = DecryptStringAES(encryptedData, key);

                var decryptedFilePath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(filePath));
                File.WriteAllText(decryptedFilePath, decryptedData);

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
        using (var keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("salt")))
        {
            return keyDerivation.GetBytes(KeySize / 8);
        }
    }

    public string EncryptStringAES(string plainText, byte[] password)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = password;
            aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string DecryptStringAES(string cipherText, byte[] password)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = password;
            aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    #endregion
}