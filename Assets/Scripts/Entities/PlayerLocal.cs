using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.Types;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerLocal : Character
    {
        private PositionNetworkEntity _currentPosition;
        private Quaternion _lastRotate;

        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var dir = transform.Find("Body").transform.forward * vertical + transform.Find("Body").transform.right * horizontal;

            _currentPosition = new PositionNetworkEntity
            {
                x = dir.x,
                y = 0,
                z = dir.z,
            };

            if (horizontal != 0 || vertical != 0)
                playerController.Move(_currentPosition);
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
                playerController.Attack("");
        }

        public override void Hit(int damage)
        {
            _hp -= damage;

            if (_hp <= 0) Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Unity Events
        void Update()
        {
            Move();
            Attack();
        }

        private async void LateUpdate()
        {
            if (_currentPosition is not null && _currentPosition.toVector3() != Vector3.zero)
                await NetworkHelper.Send<PositionNetworkEntity>(OpCodeType.PLAYER_MOVE, _currentPosition);

            if (_lastRotate != transform.rotation) {
                var rotate = new RotateNetworkEntity
                {
                    x = transform.rotation.x,
                    y = transform.rotation.y,
                    z = transform.rotation.z,
                };

                if (rotate.IsChange()) await NetworkHelper.Send<RotateNetworkEntity>(OpCodeType.PLAYER_ROTATE, rotate);
            }

        }
        #endregion
    }
}