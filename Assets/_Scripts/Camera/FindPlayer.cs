using UnityEngine;
using Cinemachine;
using Root.Player;

namespace Root.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FindPlayer : MonoBehaviour
    {
        private CinemachineVirtualCamera mainCam;

        [SerializeField] private bool debug;

        private PlayerManager player;

        private void Awake() => mainCam = GetComponent<CinemachineVirtualCamera>();

        private void Start() => InvokeRepeating(nameof(LookForPlayer), 0, 0.05f);

        private void LookForPlayer()
        {
            if (player != null) return;

            player = FindObjectOfType<PlayerManager>();

            if (player == null) return;

            Transform playerTransform = player.transform;

            mainCam.LookAt = playerTransform;
            mainCam.Follow = playerTransform;
            mainCam.transform.position = playerTransform.position;

            CancelInvoke();
        }
    }
}