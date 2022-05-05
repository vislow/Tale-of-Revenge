using UnityEngine;

namespace Root.Data
{
    [System.Serializable]
    public class SaveData
    {
        public string saveName;

        public LevelData levelData;
        public PlayerData playerData;
    }

    [System.Serializable]
    public struct PlayerData
    {
        public Vector2 lastSafePosition;
    }

    [System.Serializable]
    public struct LevelData
    {
        public int currentLevel;
    }
}