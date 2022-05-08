using UnityEngine;

namespace Root.LevelManagement.Locations
{
    [CreateAssetMenu(fileName = "LocationObject", menuName = "Scriptable Objects/Location Management/Location Object")]
    public class LocationObject : ScriptableObject
    {
        public string LocationName;
    }
}