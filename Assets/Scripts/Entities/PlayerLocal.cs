using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerLocal : Character
    {
        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var dir = transform.Find("Body").transform.forward * vertical + transform.Find("Body").transform.right * horizontal;

            var position = new PositionNetworkEntity
            {
                x = dir.x,
                y = 0,
                z = dir.z,
            };

            if(horizontal != 0 || vertical != 0)
                playerController.Move(position);
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
        #endregion
    }
}