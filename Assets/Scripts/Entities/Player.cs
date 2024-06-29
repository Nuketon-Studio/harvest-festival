using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class Player : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CharacterSO characterSO;

        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var position = new PositionEntityNetwork {
                x = horizontal,
                y = 0,
                z = vertical,
            };

            playerController.Move(position);
        }

        private void Attack() {
            if(Input.GetMouseButtonDown(0)) 
                playerController.Attack("");
        }
        #endregion

        #region Unity Events
        void Start()
        {
            playerController?.Init(characterSO);

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