using System;
using UnityEngine;

namespace Root
{
    public class CheckpointController : MonoBehaviour
    {
        public Transform spawnPoint;

        internal int id;

        public static Action<int> OnCheckpointTrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            OnCheckpointTrigger?.Invoke(id);
        }
    }
}