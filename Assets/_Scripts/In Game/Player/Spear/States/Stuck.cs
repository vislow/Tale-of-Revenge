using Root.Entities;
using Root.Systems.Input;
using UnityEngine;

namespace Root.Player.Spear
{
    [RequireComponent(typeof(Collider2D))]
    public class Stuck : BaseState
    {
        public Stuck(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager Sm => (SpearStateManager)stateMachine;

        private float spearAngleOffset = 25f;
        private Vector4 spearAnglesDefault => new Vector4(45, 135, 225, 315);
        private Vector4 spearAngles => new Vector4(
            spearAnglesDefault.x - spearAngleOffset,
            spearAnglesDefault.y + spearAngleOffset,
            spearAnglesDefault.z - spearAngleOffset,
            spearAnglesDefault.w + spearAngleOffset);

        private Collider2D solidCollider;

        public override void Enter()
        {
            base.Enter();


            Sm.SetObjectActivity(activeObj: false, stuckObj: true, returnObj: false);
            Sm.rb.velocity = Vector3.zero;

            FixSpearRotation();

            solidCollider = Sm.stuckObjects.GetComponent<Collider2D>();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            /// TODO: Make S + Spacebar to fall through platform
            solidCollider.enabled = InputManager.instance.verticalInput == -1 ? false : Sm.PlayerCollision.spearUnderPlayer;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        private void FixSpearRotation()
        {
            var angles = Sm.transform.eulerAngles;
            var zAngle = angles.z;

            if (zAngle > spearAngles.x && zAngle < 90)
            {
                zAngle = spearAngles.x;
            }
            else if (zAngle >= 90 && zAngle < spearAngles.y)
            {
                zAngle = spearAngles.y;
            }
            else if (zAngle > spearAngles.z && zAngle <= 270)
            {
                zAngle = spearAngles.z;
            }
            else if (zAngle > 270 && zAngle < spearAngles.w)
            {
                zAngle = spearAngles.w;
            }

            Sm.transform.eulerAngles = new Vector3(angles.x, angles.y, zAngle);
        }
    }
}
