using UnityEngine;

namespace Root.Entities.Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        public bool debug;
        [Space]
        public float speed;
        public Rigidbody2D rb;
    }
}