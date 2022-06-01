using Cinemachine;
using UnityEngine;

namespace Root.Player.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float centerCameraWaitTime = 5f;
        [SerializeField][Range(0, 1)] private float cameraCenterSpeed = 0.2f;

        public CinemachineVirtualCamera vcam;

        private Vector2 playerPos;
        private Vector2 latePlayerPos;
        private float centerCameraTimer;

        private void Update()
        {
            if (PlayerManager.isPlayerNull) return;

            latePlayerPos = playerPos;
            playerPos = PlayerManager.instance.playerPosition;
            centerCameraTimer -= Time.deltaTime;

            if (latePlayerPos == playerPos || PlayerManager.instance.isPlayerDead) return;

            vcam.enabled = true;
            centerCameraTimer = centerCameraWaitTime;
        }

        private void LateUpdate()
        {
            if (centerCameraTimer >= 0 && !PlayerManager.instance.isPlayerDead) return;

            vcam.enabled = false;

            float xPos = Mathf.Lerp(transform.position.x, playerPos.x, cameraCenterSpeed);
            float yPos = Mathf.Lerp(transform.position.y, playerPos.y, cameraCenterSpeed);

            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }

        public void CenterCamera()
        {
            vcam.enabled = false;
            transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
            vcam.enabled = true;
        }
    }
}