using System;
using System.IO;
using UnityEngine;

public abstract class FileDataHandler
{
    protected string dataDirPath = "";
    protected string dataFileName = "";

    public abstract GameData Load();
    public abstract void Save(GameData gameData);
    protected virtual string GetDataToLoad()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        string dataToLoad = "";
        if (File.Exists(fullPath))
        {
            try
            {
                using FileStream stream = new FileStream(fullPath, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);
                dataToLoad = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading data from file: " + fullPath + "\n" + e);
            }
        }
        return dataToLoad;
    }
}
