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

        private void Awake() => mainCam = GetComponent<CinemachineVirtualCamera>();

        private void Start() => InvokeRepeating(nameof(LookForPlayer), 0, 0.05f);

        private void LookForPlayer()
        {
            if (!PlayerManager.isPlayerNull) return;

            Transform playerTransform = PlayerManager.instance.transform;

            mainCam.Follow = playerTransform;
            mainCam.transform.position = playerTransform.position;

            PlayerManager.
            instance.components.playerCamera = cameraController;

            CancelInvoke();
        }
    }
}