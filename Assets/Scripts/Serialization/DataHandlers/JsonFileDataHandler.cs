using System;
using System.IO;
using UnityEngine;
using Zenject;

public class JsonFileDataHandler : FileDataHandler
{
    [Inject] private readonly IEncryptor _encryptor;

    private bool _useEncryption;

    public JsonFileDataHandler(string DirPath, string FileName, bool useEncryption)
    {
        dataDirPath = DirPath;
        dataFileName = FileName;
        _useEncryption = useEncryption;
    }

    public override GameData Load()
    {
        GameData loadedData = null;
        try
        {
            string dataToLoad = GetDataToLoad();

            if (_useEncryption)
            {
                dataToLoad = _encryptor.EncryptDecrypt(dataToLoad);
            }

            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while loading data from file: " + dataFileName + "\n" + e);
        }
        return loadedData;
    }

    public override void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);

            if (_useEncryption)
            {
                dataToStore = _encryptor.EncryptDecrypt(dataToStore);
            }

            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving data to file: " + fullPath + "\n" + e);
        }
    }
}