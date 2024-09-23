public class XorEncryptor : IEncryptor
{
    private readonly string _encryptionKey = "pass";

    public string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionKey[i % _encryptionKey.Length]);
        }
        return modifiedData;
    }
}
