namespace RepoVault.Application.Encryption;

public interface IEncryptionService
{
    public Task EncryptFolder(string folderPath, string password);
    public Task DecryptFolder(string folderPath, string password);
    public byte[] GenerateKey(string password);
    public string EncryptStringAES(string plainText, byte[] password);
    public string DecryptStringAES(string cipherText, byte[] password);
}