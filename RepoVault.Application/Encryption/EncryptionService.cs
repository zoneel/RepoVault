using System.Security.Cryptography;
using System.Text;

namespace RepoVault.Application.Encryption;

public class EncryptionService : IEncryptionService
{
    private const int KeySize = 256; 

    public async Task EncryptFolder(string folderPath, string password)
    {
        try
        {
            byte[] key = GenerateKey(password);

            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

            foreach (string filePath in jsonFiles)
            {
                string jsonData = File.ReadAllText(filePath);

                string encryptedData = EncryptStringAES(jsonData, key);

                string encryptedFilePath = filePath + ".encrypted";
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
            byte[] key = GenerateKey(password);

            string[] encryptedJsonFiles = Directory.GetFiles(folderPath, "*.json.encrypted");

            foreach (string filePath in encryptedJsonFiles)
            {
                string encryptedData = File.ReadAllText(filePath);

                string decryptedData = DecryptStringAES(encryptedData, key);

                string decryptedFilePath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(filePath));
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
        using (Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("salt")))
        {
            return keyDerivation.GetBytes(KeySize / 8); 
        }
    }

    public string EncryptStringAES(string plainText, byte[] password)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = password;
            aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
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
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = password;
            aesAlg.IV = new byte[16]; // Initialization vector (IV) is all zeros to make it simple

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

}

