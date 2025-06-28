using UnityEngine;

namespace Root
{
    public class SpawnObject : MonoBehaviour
    {
        public void InstantiateObject(GameObject objectToInstantiate) => Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
    }
}
