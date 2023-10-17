namespace RepoVault.Application.Encryption;

public interface IEncryptionService
{
    public Task EncryptFolderAsync(string folderPath, string password);
    public Task DecryptFolderAsync(string folderPath, string password);
    public byte[] GenerateKey(string password);
    public string EncryptStringAes(string plainText, byte[] password);
    public string DecryptStringAes(string cipherText, byte[] password);
}