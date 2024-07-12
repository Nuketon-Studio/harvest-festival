using System;
using HarvestFestival.States;
using HarvestFestival.Types;
using Unity.VisualScripting;
using UnityEngine;

namespace HarvestFestival.Entities
{
    public class Skin : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Header("Events")]
        public Action<Collision> OnCollisionEnterCallback;

        private Action _attackCallback;
        private Action _jumpCallback;

        public void OnChangeState(PlayerStateType state)
        {
            switch (state)
            {
                case PlayerStateType.Walk:
                    Walk(true);
                    break;
                case PlayerStateType.Walk_Stop:
                    Walk(false);
                    break;
                case PlayerStateType.Attack:
                    animator?.SetTrigger("Attack");
                    break;
                case PlayerStateType.Jump:
                    animator?.SetTrigger("Jump");
                    break;
            }
        }

        #region Animations
        private void Walk(bool isWalk)
        {
            animator?.SetBool("Walk", isWalk);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter(Collision other)
        {
            if (OnCollisionEnterCallback is not null) OnCollisionEnterCallback(other);
        }
        #endregion

        #region Init Callbacks
        public void WaitMomentAnimationToAttack(Action callback)
        {
            _attackCallback = callback;
        }

        public void WaitMomentAnimationToJump(Action callback)
        {
            _jumpCallback = callback;
        }
        #endregion

        #region Events trigged on animator

        public void AnimationStepAttack()
        {
            if (_attackCallback is not null) _attackCallback();
        }

        public void AnimationStepJump()
        {
            if (_jumpCallback is not null) _jumpCallback();
        }
        #endregion
    }
}