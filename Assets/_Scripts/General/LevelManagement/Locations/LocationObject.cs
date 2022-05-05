using UnityEngine;

namespace Root.LevelManagement.Locations
{
    [CreateAssetMenu(fileName = "LocationObject", menuName = "Scriptable Objects/LocationObject")]
    public class LocationObject : ScriptableObject
    {
        public string LocationName;
    }
}