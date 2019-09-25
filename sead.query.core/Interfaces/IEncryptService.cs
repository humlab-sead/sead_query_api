namespace SeadQueryCore
{
    public interface IEncryptService
    {
        string Decrypt(string text, string decryptKey);
        string Encrypt(string text, string encryptKey);
        string ToMD5(string text);
    }
}