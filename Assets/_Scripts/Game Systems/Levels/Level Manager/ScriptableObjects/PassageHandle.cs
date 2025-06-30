using UnityEngine;

namespace Root.Levels
{
    public class PassageHandle : ScriptableObject
    {
        [Header("This Passage")]
        public string passageName;
        public int passageId;

        [Header("Target Passage")]
        public SceneHandle targetScene;
        public int targetPassageId;
    }
}