using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DataPersistenceManager : MonoBehaviour
{
    public string FileName => _fileName;
    public bool UseEncryption => _useEncryption;

    [Inject] private FileDataHandler _dataHandler; 

    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;

    private List<IDataPersistence> _dataPersistenceObjects;
    private GameData _gameData;

    private void Start()
    {
        _dataPersistenceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if (_gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.SaveData(_gameData);
        }
        _dataHandler.Save(_gameData);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }
}
