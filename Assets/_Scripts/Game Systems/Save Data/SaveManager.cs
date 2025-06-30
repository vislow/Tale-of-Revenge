using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Root.Data.Saving
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;

        public static Action OnPreGameSaved;

        [SerializeField] private bool debug;
        [SerializeField] private bool saveSystemActive = true;

        internal SaveData currentSave;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }

        public void Save(SaveNames saveName)
        {
            if (saveSystemActive) return;

            if (!File.Exists(GetFilePath(saveName)))
            {
                currentSave = new SaveData();
                currentSave.saveName = saveName.ToString();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            FileStream stream = new FileStream(GetFilePath(saveName), FileMode.Create);

            serializer.Serialize(stream, currentSave);
            stream.Close();

            if (!debug) return;

            Debug.Log($"Saved {saveName}");
        }

        public void Save()
        {
            if (saveSystemActive) return;

            OnPreGameSaved?.Invoke();

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            FileStream stream = new FileStream(GetFilePath(currentSave.saveName), FileMode.Create);

            serializer.Serialize(stream, currentSave);
            stream.Close();

            if (!debug) return;

            Debug.Log($"Saved {currentSave.saveName}");
        }

        public void Load(SaveNames saveName)
        {
            if (saveSystemActive) return;

            if (!File.Exists(GetFilePath(saveName))) return;

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            FileStream stream = new FileStream(GetFilePath(saveName), FileMode.Open);

            currentSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            if (!debug) return;

            Debug.Log($"Loaded {currentSave.saveName}");
        }

        public void ClearSaveData(SaveNames saveName)
        {
            if (saveSystemActive) return;

            if (!File.Exists(GetFilePath(saveName))) return;

            File.Delete(GetFilePath(saveName));
        }

        private string GetFilePath(SaveNames saveName) => Application.persistentDataPath + "/" + saveName + ".save";

        private string GetFilePath(string saveName) => Application.persistentDataPath + "/" + saveName + ".save";

        public static bool GetSaveExists(SaveNames saveName) => File.Exists(instance.GetFilePath(saveName));
    }
}