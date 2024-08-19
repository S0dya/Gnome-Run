#if UNITY_WEBGL
using YG;

namespace Saving
{
    public class YandexSave : ISaveSystem
    {
        public void Save(GameData gameData)
        {
            YandexGame.savesData = ConvertToSavesYG(gameData);
            YandexGame.SaveProgress();
        }
        public GameData Load()
        {
            return ConvertToGameData(YandexGame.savesData);
        }

        private SavesYG ConvertToSavesYG(GameData gameData)
        {
            var savesYG = new SavesYG
            {
                IntDict = gameData.IntDict,
                IntsDict = gameData.IntsDict,
            };

            return savesYG;
        }
        private GameData ConvertToGameData(SavesYG savesYg)
        {
            var gameData = new GameData
            {
                IntDict = savesYg.IntDict,
                IntsDict = savesYg.IntsDict,
            };

            return gameData;
        }
    }
}
#endif
