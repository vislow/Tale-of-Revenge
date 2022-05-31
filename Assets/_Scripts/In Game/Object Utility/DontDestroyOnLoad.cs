using UnityEngine;

namespace Root.ObjectManagement
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(this.gameObject);
    }
}
