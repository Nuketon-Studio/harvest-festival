using System;
using HarvestFestival.States;
using HarvestFestival.Types;
using UnityEngine;

namespace HarvestFestival.Entities
{
    public class Skin : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Action<GameObject> OnCollisionEnterCallback;

        private Action _attackCallback;
        private Action _jumpCallback;

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterCallback(other.gameObject);
        }

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
        public void Talk()
        {
            Debug.Log("Opa acho que rolou agora emmmm");
        }

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