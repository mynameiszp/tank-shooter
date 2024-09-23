using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Zenject;

public class XmlFileDataHandler : FileDataHandler
{
    [Inject] private readonly IEncryptor _encryptor;

    private string _dataDirPath = "";
    private string _dataFileName = "";
    private bool _useEncryption;

    public XmlFileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
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

            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using StringReader stringReader = new StringReader(dataToLoad);
            loadedData = (GameData)serializer.Deserialize(stringReader);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while loading data from file: " + dataFileName + "\n" + e);
        }
        return loadedData;
    }

    public override void Save(GameData gameData)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            string dataToStore;
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, gameData);
                dataToStore = stringWriter.ToString();
            }

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
