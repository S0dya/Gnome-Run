using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Saving
{
    public class JsonSave : ISaveSystem
    {
        private const string SaveKey = "game_data";

        private readonly string _filePath = Application.persistentDataPath + "/data.json";

        public void Save(GameData data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, EncryptionHelper.Encrypt(jsonData));
        }

        public GameData Load()
        {
            if (!File.Exists(_filePath))
            {
                Debug.Log("File doesn't exist"); return null;
            }

            string encryptedData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<GameData>(EncryptionHelper.Decrypt(encryptedData));
        }
    }

    static class EncryptionHelper
    {
        private static readonly string key = "Sg9f0db5ZQxP47j4bScU/J2z4oPzuvxRff9DJsK/3NU=";
        private static readonly string iv = "0dJ+lBri8gRa2zZdlYc5Ew==";

        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.IV = Convert.FromBase64String(iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new(cs))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.IV = Convert.FromBase64String(iv);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new(Convert.FromBase64String(cipherText)))
                using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
