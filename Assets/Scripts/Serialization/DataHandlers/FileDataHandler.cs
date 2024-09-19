public interface FileDataHandler
{
    GameData Load();
    void Save(GameData gameData);
}
