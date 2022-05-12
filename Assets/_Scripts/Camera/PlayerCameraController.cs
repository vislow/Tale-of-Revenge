using UnityEngine;
using Cinemachine;
using Root.Player;

namespace Root.Cameras
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private float timeBeforeCenterCamera = 5f;
        [SerializeField][Range(0, 1)] private float cameraCenterSpeed = 0.2f;

        private CinemachineVirtualCamera vcam;
        private PlayerManager player;

        private Transform parentTransform;

        private Vector2 playerPos;
        private Vector2 latePlayerPos;
        private float centerCameraTimer;

        private void Awake()
        {
            vcam = GetComponent<CinemachineVirtualCamera>();
            parentTransform = transform.parent.transform;
        }

        private void Start() => player = PlayerManager.instance;

        private void Update()
        {
            if (player == null) return;

            latePlayerPos = playerPos;
            playerPos = player.components.center.transform.position;
            centerCameraTimer -= Time.deltaTime;

            if (latePlayerPos == playerPos || player.components.deathManager.dead) return;

            vcam.enabled = true;
            centerCameraTimer = timeBeforeCenterCamera;
        }

        private void LateUpdate()
        {
            if (centerCameraTimer >= 0 && !player.components.deathManager.dead) return;

            vcam.enabled = false;

            float xPos = Mathf.Lerp(parentTransform.position.x, playerPos.x, cameraCenterSpeed);
            float yPos = Mathf.Lerp(parentTransform.position.y, playerPos.y, cameraCenterSpeed);

            parentTransform.position = new Vector3(xPos, yPos, parentTransform.position.z);
        }

        public void CenterCamera(bool lerp = false)
        {
            vcam.enabled = false;

            if (lerp)
            {
                float xPos = Mathf.Lerp(parentTransform.position.x, playerPos.x, cameraCenterSpeed);
                float yPos = Mathf.Lerp(parentTransform.position.y, playerPos.y, cameraCenterSpeed);

                parentTransform.position = new Vector3(xPos, yPos, parentTransform.position.z);
            }
            else
            {
                parentTransform.position = new Vector3(playerPos.x, playerPos.y, parentTransform.position.z);
            }

            vcam.enabled = true;
        }
    }
}