using System.IO;
using UnityEngine;

namespace SaveLoadSystem
{
    
    public static class SaveGameManager
    {
        public static SaveData CurrentSaveData = new SaveData();

        public const string SaveDirectory = "/SaveData/";
        public const string FileName = "SaveGame.txt";

        public static bool SaveGame()
        {
            var directory = Application.persistentDataPath + SaveDirectory;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(directory + FileName, json);

            GUIUtility.systemCopyBuffer = directory;

            return true;
        }

        public static void LoadGame()
        {
            string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
            SaveData tempData = new SaveData();

            if(File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogError("Save file does not exist!");
            }

            CurrentSaveData = tempData;
        }
    }
}
