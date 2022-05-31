using UnityEngine;
using UnityEngine.Events;

namespace Root.ObjectManagement
{
    public class DestroySelfObject : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnDestroy;

        public void DestroySelf()
        {
            OnDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}
