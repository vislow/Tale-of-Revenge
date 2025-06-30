using System.Collections.Generic;
using UnityEngine;

namespace Root.Levels
{
    [CreateAssetMenu(fileName = "NewSceneHandle", menuName = "Scriptable Objects/Level Management/Scene Handle")]
    public class SceneHandle : ScriptableObject
    {
        public Object scene;
        public string sceneName;
        [SerializeField] private List<PassageHandle> passageList = new List<PassageHandle>();
        public List<PassageHandle> passages { get => passageList; set => passageList = value; }
    }
}