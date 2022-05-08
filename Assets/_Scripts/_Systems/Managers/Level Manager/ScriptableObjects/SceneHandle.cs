using System.Collections.Generic;

using UnityEngine;

namespace Root.LevelLoading.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSceneHandle", menuName = "Scriptable Objects/Level Management/Scene Handle")]
    public class SceneHandle : ScriptableObject
    {
        public Object scene;

        [SerializeField] private List<PassageHandle> passageList = new List<PassageHandle>();
        public List<PassageHandle> passages { get => passageList; set => passageList = value; }
    }
}