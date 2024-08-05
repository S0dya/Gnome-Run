
namespace Saving
{
    public interface ISaveSystem
    {
        void Save(GameData gameData);
        GameData Load();
    }
}
