using UnityEngine;
using Cinemachine;
using Player;

namespace Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FindPlayer : MonoBehaviour
    {
        /// <Description> Variables </Description>

        private CinemachineVirtualCamera mainCam;

        [SerializeField] private bool debug;

        private PlayerManager player;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake() => mainCam = GetComponent<CinemachineVirtualCamera>();

        private void Start() => InvokeRepeating(nameof(LookForPlayer), 0, 0.05f);

        /// <Description> Custom Methods </Description>

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