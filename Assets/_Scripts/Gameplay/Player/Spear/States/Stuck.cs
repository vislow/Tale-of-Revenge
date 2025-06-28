using Root.Entities;
using Root.Input;
using UnityEngine;

namespace Root.Player.Spear
{
    [RequireComponent(typeof(Collider2D))]
    public class Stuck : BaseState
    {
        public Stuck(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager sm => (SpearStateManager)stateMachine;

        private float spearAngleOffset = 25f;
        private Vector4 spearAnglesDefault => new Vector4(45, 135, 225, 315);
        private Vector4 spearAngles => new Vector4(
            spearAnglesDefault.x - spearAngleOffset,
            spearAnglesDefault.y + spearAngleOffset,
            spearAnglesDefault.z - spearAngleOffset,
            spearAnglesDefault.w + spearAngleOffset);

        private float spearReturnTimer;

        public override void Enter()
        {
            base.Enter();

            sm.SetObjectActivity(activeObj: false, stuckObj: true, returnObj: false);
            sm.rb.linearVelocity = Vector3.zero;

            FixSpearRotation();

            spearReturnTimer = sm.spearReturnTime;
            sm.stuckObjectRenderer.color = Color.white;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            spearReturnTimer -= Time.deltaTime;

            sm.stuckObjectRenderer.color = Color.Lerp(sm.stuckObjectRenderer.color, sm.fadedColor, 0.05f / sm.spearReturnTime);

            if (spearReturnTimer < 0)
            {
                sm.ReturnToPlayer();
            }

            sm.stuckObjectCollider.enabled = InputManager.instance.verticalInput == -1 ? false : sm.player.components.collision.spearUnderPlayer;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        private void FixSpearRotation()
        {
            var angles = sm.transform.eulerAngles;
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

            sm.transform.eulerAngles = new Vector3(angles.x, angles.y, zAngle);
        }
    }
}
