using Root.Data.Saving;

namespace Root.Data
{
    public class SaveHelper
    {
        public static SaveData GetCurrentSave() => GetCurrentSaveAvailable() ? SaveManager.instance.currentSave : new SaveData();

        public static bool GetCurrentSaveAvailable()
        {
            SaveManager saveManager = SaveManager.instance;

            return saveManager != null && saveManager.currentSave != null;
        }

        private static SaveManager saveManager => SaveManager.instance;

        public static void Save()
        {
            if (!GetCurrentSaveAvailable()) return;

            saveManager.Save();
        }

        public static void Save(SaveNames saveName) => saveManager.Save(saveName);

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

        public static bool GetSaveExists(SaveNames saveName) => SaveManager.GetSaveExists(saveName);
    }
}