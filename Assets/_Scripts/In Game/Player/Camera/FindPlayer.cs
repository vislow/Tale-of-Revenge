using Cinemachine;
using UnityEngine;

namespace Root.Player.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FindPlayer : MonoBehaviour
    {
        private CinemachineVirtualCamera mainCam;

        [SerializeField] private bool debug;
        [SerializeField] private CameraController cameraController;

        private PlayerManager player;

        private void Awake() => mainCam = GetComponent<CinemachineVirtualCamera>();

        private void Start() => InvokeRepeating(nameof(LookForPlayer), 0, 0.05f);

        private void LookForPlayer()
        {
            if (player != null) return;

            player = PlayerManager.instance;

            if (player == null) return;

            Transform playerTransform = player.transform;

            mainCam.LookAt = playerTransform;
            mainCam.Follow = playerTransform;
            mainCam.transform.position = playerTransform.position;
            player.components.playerCamera = cameraController;

            CancelInvoke();
        }
    }
}