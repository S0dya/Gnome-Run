#if UNITY_WEBGL
using System.Collections.Generic;
using YG;
using UnityEngine;

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
            return ConvertToGameData();
        }

        private SavesYG ConvertToSavesYG(GameData gameData)
        {
            SeparateDictionaryIntoLists(gameData.IntDict, out List<string> intDictKeys, out List<int> intDictValues);
            SeparateDictionaryIntoLists(gameData.BoolDict, out List<string> boolDictKeys, out List<bool> boolDictValues);

            YandexGame.savesData.IntDictKeys = intDictKeys.ToArray(); YandexGame.savesData.IntDictValues = intDictValues.ToArray();
            YandexGame.savesData.IntsShopUnlockedCharacters = gameData.IntsDict[Settings.ShopUnlockedCharacters_Key];
            YandexGame.savesData.BoolDictKeys = boolDictKeys.ToArray(); YandexGame.savesData.BoolDictValues = boolDictValues.ToArray();

            return YandexGame.savesData;
        }
        private GameData ConvertToGameData()
        {
            var savesYg = YandexGame.savesData;
            if (savesYg.IntDictKeys.Length == 0) return null;

            var gameData = new GameData
            {
                IntDict = FormDictionaryFromKeysAndValues(savesYg.IntDictKeys, savesYg.IntDictValues),
                IntsDict = new Dictionary<string, int[]>
                {
                    { Settings.ShopUnlockedCharacters_Key, savesYg.IntsShopUnlockedCharacters },
                },
                BoolDict = FormDictionaryFromKeysAndValues(savesYg.BoolDictKeys, savesYg.BoolDictValues),
            };

            return gameData;
        }

        private void SeparateDictionaryIntoLists<T>(Dictionary<string, T> dict, out List<string> keys, out List<T> values)
        {
            keys = new();
            values = new();

            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
        private Dictionary<string, T> FormDictionaryFromKeysAndValues<T>(string[] keys, T[] values)
        {
            var result = new Dictionary<string, T>();

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != "" && !result.ContainsKey(keys[i]))
                {
                    result.Add(keys[i], values[i]);
                }
            }

            return result;
        }
    }
}
#endif
