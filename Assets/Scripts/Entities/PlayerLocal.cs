using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerLocal : Character
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CharacterSO characterSO;

        private int _hp;

        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var position = new PositionEntityNetwork {
                x = horizontal,
                y = 0,
                z = vertical * Mathf.Sign(transform.position.z - Camera.main.transform.position.z),
            };

            playerController.Move(position);
        }

        private void Attack() {
            if(Input.GetMouseButtonDown(0)) 
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
        void Start()
        {
            playerController?.Init(characterSO);
            _hp = characterSO.hp;

            playerController = GetComponent<PlayerController>();
        }
        void Update()
        {
            Move();
            Attack();
        }
        #endregion
    }
}