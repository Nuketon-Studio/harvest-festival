using System.Text;
using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.SO;
using HarvestFestival.Types;
using Nakama;
using Nakama.TinyJson;
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

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);

            switch (matchState.OpCode)
            {
                case OpCodeType.PLAYER_MOVE:
                    PositionNetworkEntity position = JsonParser.FromJson<PositionNetworkEntity>(jsonUtf8);

                    playerController.Move(position);
                    break;
                case OpCodeType.PLAYER_ROTATE:
                    RotateNetworkEntity rotate = JsonParser.FromJson<RotateNetworkEntity>(jsonUtf8);

                    transform.rotation = rotate.toEuler();
                    break;
            }
        }
        #endregion

        #region Unity Events
        void Update()
        {
            Move();
            Attack();
        }

        void Start()
        {
            GameManager.Instance.Connection.Socket.ReceivedMatchState += OnReceiveMatchState;
        }
        #endregion
    }
}