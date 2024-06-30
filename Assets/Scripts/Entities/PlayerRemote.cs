using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerRemote : Character
    {
        #region Actions
        private void Move()
        {
            // playerController.Move(position);
        }

        private void Attack() {
            // if(Input.GetMouseButtonDown(0)) 
            //     playerController.Attack("");
        }

        public override void Hit(int damage) {
            _hp -= damage;

            if(_hp <= 0) Die();
        }

        private void Die() {
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