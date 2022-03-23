using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Locations {
    [CreateAssetMenu(fileName = "LocationObject", menuName = "Scriptable Objects/LocationObject")]
    public class LocationObject : ScriptableObject {
        public string LocationName;
    }
}