using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {
    public Transform spawnPoint;

    internal int id;

    public static Action<int> OnCheckpointTrigger;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            OnCheckpointTrigger?.Invoke(id);
        }
    }
}
