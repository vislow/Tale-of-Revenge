using Root.Data.Saving;

namespace Root.Data
{
    public class SaveHelper
    {
        public static SaveData CurrentSave => CurrentSaveAvailable ? SaveManager.instance.currentSave : new SaveData();

        public static bool CurrentSaveAvailable
        {
            get
            {
                SaveManager saveManager = SaveManager.instance;

                return saveManager != null && saveManager.currentSave != null;
            }
        }

        private static SaveManager saveManager => SaveManager.instance;

        public static void Save()
        {
            if (!CurrentSaveAvailable) return;

            saveManager.Save();
        }

        public static void Save(SaveNames saveName)
        {
            if (!CurrentSaveAvailable) return;

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