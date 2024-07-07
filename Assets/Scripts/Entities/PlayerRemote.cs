using System.Collections.Generic;
using System.Text;
using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.SO;
using HarvestFestival.States;
using HarvestFestival.Types;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerRemote : Character
    {
        private PlayerStateMachine _state;


        public override void Init(CharacterSO character, string userId)
        {
            base.Init(character, userId);

            _state = new PlayerStateMachine();
            _state.OnChangeState += _skin.OnChangeState;
        }

        #region Actions
        private void Attack()
        {
            // if(Input.GetMouseButtonDown(0)) 
            //     playerController.Attack("");
        }

        public override void Hit(int damage)
        {
            _hp -= damage;

            UIHelper.Hit(transform.Find("Skin").gameObject.transform);
            
            if (_hp <= 0) Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            if (_isOffline) return;

            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);
            var content = JsonParser.FromJson<Dictionary<string, string>>(jsonUtf8);

            if (content.ContainsKey("userId") && content["userId"] != _userId) return;

            switch (matchState.OpCode)
            {
                case OpCodeType.PLAYER_MOVE:
                    PositionNetworkEntity position = JsonParser.FromJson<PositionNetworkEntity>(jsonUtf8);

                    playerController.Move(position);
                    break;
                case OpCodeType.PLAYER_ROTATE:
                    RotateNetworkEntity rotate = JsonParser.FromJson<RotateNetworkEntity>(jsonUtf8);

                    transform.eulerAngles = rotate.toVector3();
                    break;
                case OpCodeType.PLAYER_ATTACK_LIGHT:
                    AttackNetworkEntity attackLight = JsonParser.FromJson<AttackNetworkEntity>(jsonUtf8);

                    // TODO - tentar pegar ou instanciar o prefab do projetil usando o nome que ta vindo,
                    // achei melhor no SO adicionar o prefab do que o nome para poder chamar no reuquire, quem sabe posso montar 
                    // de forma automatica para pdoer usar aqui e instanciar via resource, penso melhor depois.
                    // playerController.Attack(attackLight.toVector3(), attackLight.prefabName);
                    break;
            }
        }
        #endregion

        #region Unity Events

        protected override void Start()
        {
            base.Start();

            if (_isOffline)
            {
                Init(stats, "");
                return;
            }

            GameManager.Instance.Connection.Socket.ReceivedMatchState += OnReceiveMatchState;
        }

        void OnDestroy()
        {
            if (_isOffline) return;

            GameManager.Instance.Connection.Socket.ReceivedMatchState -= OnReceiveMatchState;
        }
        #endregion
    }
}