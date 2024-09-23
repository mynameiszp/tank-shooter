using TMPro;
using UnityEngine;
using Zenject;

public class GameUIManager : MonoBehaviour
{
    [Inject] private EnemySpawnManager _enemySpawnManager;

    [SerializeField] private TextMeshProUGUI _enemiesCount;

    private void Update()
    {
        UpdateEnemiesCount();
    }

    private void UpdateEnemiesCount()
    {
        if (_enemiesCount == null) return;
        _enemiesCount.text = string.Format("{0}/{1}", _enemySpawnManager.GetAliveEnemiesCount(), _enemySpawnManager.GetTotalEnemiesCount());
    }

    public void Quit()
    {
        Application.Quit();
    }
}