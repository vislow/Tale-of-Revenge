using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player;

namespace Cameras {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FindPlayer : MonoBehaviour {
        private CinemachineVirtualCamera mainCam;

        [SerializeField] private bool debug;

        private PlayerManager player;

        private void Awake() {
            mainCam = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start() {
            InvokeRepeating(nameof(LookForPlayer), 0, 0.05f);
        }

        private void LookForPlayer() {
            if (debug)
                Debug.Log("Looking for player");

            if (player != null) return;

            player = FindObjectOfType<PlayerManager>();

            Transform playerTransform = player.transform;

            mainCam.LookAt = playerTransform;
            mainCam.Follow = playerTransform;
            mainCam.transform.position = playerTransform.position;

            if (player != null) return;

            CancelInvoke();
        }
    }
}