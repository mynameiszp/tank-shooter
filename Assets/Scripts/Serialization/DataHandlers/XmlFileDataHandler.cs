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

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEncryption)
                {
                    dataToLoad = _encryptor.EncryptDecrypt(dataToLoad);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                using (StringReader stringReader = new StringReader(dataToLoad))
                {
                    loadedData = (GameData)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData gameData)
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

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving data to file: " + fullPath + "\n" + e);
        }
    }
}
