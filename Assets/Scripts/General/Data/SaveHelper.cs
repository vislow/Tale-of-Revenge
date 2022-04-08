using UnityEngine;
using Data.Saving;

namespace Data
{
    public class SaveHelper
    {
        /// <Description> Variables </Description>

        public static SaveData CurrentSave => CurrentSaveAvailable
                ? SaveManager.instance.currentSave : new SaveData();

        public static bool CurrentSaveAvailable
        {
            get
            {
                SaveManager saveManager = SaveManager.instance;

                return saveManager == null || saveManager.currentSave == null;
            }
        }

        private static SaveManager saveManager => SaveManager.instance;

        /// <Description> Methods </Description>

        public static void Save()
        {
            if (saveManager == null) return;

            saveManager.Save();
        }

        public static void Save(SaveNames saveName)
        {
            if (saveManager == null) return;

            saveManager.Save(saveName);
        }

        public static void Load(SaveNames saveName)
        {
            if (saveManager == null) return;

            saveManager.Load(saveName);
        }

        public static void ClearSaveData(SaveNames saveName)
        {
            if (saveManager == null) return;

            saveManager.ClearSaveData(saveName);
        }

        public static bool GetSaveExists(SaveNames saveName)
        {
            return SaveManager.GetSaveExists(saveName);
        }
    }
}