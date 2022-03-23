using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class MouseWorldPosition : MonoBehaviour
    {
        /// <Description> Variables </Description>
        public static MouseWorldPosition instance;

        [SerializeField] private Transform cursor;
        [SerializeField] private Camera mainCam;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        
        /// <Description> Custom Methods </Description>

        public Vector3 GetMouseWorldPosition()
        {
            Vector2 mousePosition = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            return mousePosition;
        }
    }
}