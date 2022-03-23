using UnityEngine;

namespace ObjectManagement {
    public class SpawnObject : MonoBehaviour {
        public void InstantiateObject(GameObject objectToInstantiate) {
            Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
        }
    }
}
