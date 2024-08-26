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
            return ConvertToGameData(YandexGame.savesData);
        }

        private SavesYG ConvertToSavesYG(GameData gameData)
        {
            SeparateDictionaryIntoLists(gameData.IntDict, out List<string> intDictKeys, out List<int> intDictValues);
            SeparateDictionaryIntoLists(gameData.IntsDict, out List<string> intsDictKeys, out List<int[]> intsDictValues);
            SeparateDictionaryIntoLists(gameData.BoolDict, out List<string> boolDictKeys, out List<bool> boolDictValues);

            var savesYG = new SavesYG
            {
                IntDictKeys = intDictKeys.ToArray(),
                IntDictValues = intDictValues.ToArray(),

                IntsDictKeys = intsDictKeys.ToArray(),
                IntsDictValues = intsDictValues.ToArray(),

                BoolDictKeys = boolDictKeys.ToArray(),
                BoolDictValues = boolDictValues.ToArray(),
            };

            return savesYG;
        }
        private GameData ConvertToGameData(SavesYG savesYg)
        {
            if (savesYg.isFirstSession || savesYg.IntsDictKeys == null) return null;

            var gameData = new GameData
            {
                IntDict = FormDictionaryFromKeysAndValues(savesYg.IntDictKeys, savesYg.IntDictValues),
                IntsDict = FormDictionaryFromKeysAndValues(savesYg.IntsDictKeys, savesYg.IntsDictValues),
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
                result.Add(keys[i], values[i]);
            }

            return result;
        }
    }
}
#endif
