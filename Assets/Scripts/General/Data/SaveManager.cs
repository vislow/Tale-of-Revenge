using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
    public class SaveManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

        public static SaveManager instance;
        
        public static Action OnPreGameSaved;

        [SerializeField] private bool debug;

        internal SaveData currentSave;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <Description> Custom Methods </Description>

        public void Save(SaveNames saveName)
        {
            if (!File.Exists(GetFilePath(saveName)))
            {
                currentSave = new SaveData();
                currentSave.saveName = saveName.ToString();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

            FileStream stream = new FileStream(GetFilePath(saveName), FileMode.Create);

            serializer.Serialize(stream, currentSave);
            stream.Close();

            if (debug)
                Debug.Log($"Saved {saveName}");
        }

        public void Save()
        {
            OnPreGameSaved?.Invoke();

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

            FileStream stream = new FileStream(GetFilePath(currentSave.saveName), FileMode.Create);

            serializer.Serialize(stream, currentSave);
            stream.Close();

            if (debug)
                Debug.Log($"Saved {currentSave.saveName}");
        }

        public void Load(SaveNames saveName)
        {
            if (!File.Exists(GetFilePath(saveName))) return;

            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

            FileStream stream = new FileStream(GetFilePath(saveName), FileMode.Open);

            currentSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            if (debug)
                Debug.Log($"Loaded {currentSave.saveName}");
        }

        public void ClearSaveData(SaveNames saveName)
        {
            if (!File.Exists(GetFilePath(saveName))) return;

            File.Delete(GetFilePath(saveName));
        }

        private string GetFilePath(SaveNames saveName)
        {
            return Application.persistentDataPath + "/" + saveName + ".save";
        }

        private string GetFilePath(string saveName)
        {
            return Application.persistentDataPath + "/" + saveName + ".save";
        }

        public static bool GetSaveExists(SaveNames saveName)
        {
            return File.Exists(instance.GetFilePath(saveName));
        }
    }

    public enum SaveNames
    {
        Save1,
        Save2,
        Save3,
        Save4,
    }
}