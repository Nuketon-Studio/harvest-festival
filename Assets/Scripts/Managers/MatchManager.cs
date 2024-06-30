using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarvestFestival.Entities;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.Types;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace HarvestFestival.Managers
{
    class MatchManager : MonoBehaviour
    {
        [field: SerializeField] public List<UserNetworkEntity> players { get; private set; } = new List<UserNetworkEntity>();

        [SerializeField] private GameObject playerRemotePrefab;
        [SerializeField] private GameObject playerLocalPrefab;

        public string MatchId { get; private set; }
        private List<UserLobbyNetworkEntity> _usersLobbyInfo;

        #region Gets/Sets
        public void SetMatchId(string matchId) => MatchId = matchId;
        public void SetPlayerLobbyInfo(List<UserLobbyNetworkEntity> usersLobbyInfo) => _usersLobbyInfo = usersLobbyInfo;
        #endregion

        public async void HostAddPlayers(List<UserLobbyNetworkEntity> usersLobby)
        {
            foreach (var userLobby in usersLobby)
            {
                Character character;
                bool isLocal = GameManager.Instance.IsLocal(userLobby.userId);

                if (isLocal)
                    character = Instantiate(playerLocalPrefab).GetComponent<PlayerLocal>();
                else
                    character = Instantiate(playerRemotePrefab).GetComponent<PlayerRemote>();

                character.Init(userLobby.characterStats);

                var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
                if (camera && isLocal) camera.Attachment(character.transform);

                players.Add(new UserNetworkEntity
                {
                    character = character,
                    isLocal = isLocal,
                    userId = userLobby.userId,
                    account = userLobby.account
                });

                await NetworkHelper.Send<SpawnPlayerNetworkEntity>(OpCodeType.MATCH_SPAWN_PLAYER, new SpawnPlayerNetworkEntity
                {
                    x = character.transform.position.x,
                    y = character.transform.position.y,
                    z = character.transform.position.z,
                    userId = userLobby.userId,
                });
            }
        }

        public void AddPlayerRemotely(SpawnPlayerNetworkEntity playerRemote)
        {
            var exists = players.Exists(f => f.userId == playerRemote.userId);

            if (exists) return;

            var userLobbyInfo = _usersLobbyInfo.Find(f => f.userId == playerRemote.userId);

            if (userLobbyInfo is null) return;

            bool isLocal = GameManager.Instance.IsLocal(playerRemote.userId);
            Character character;
            
            if (isLocal)
                character = Instantiate(playerLocalPrefab).GetComponent<PlayerLocal>();
            else
                character = Instantiate(playerRemotePrefab).GetComponent<PlayerRemote>();

            character.Init(userLobbyInfo.characterStats);
            character.transform.position = playerRemote.toVector3();

            var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
            
            if (camera && isLocal) camera.Attachment(character.transform);

            players.Add(new UserNetworkEntity
            {
                character = character,
                isLocal = false,
                userId = playerRemote.userId,
                account = userLobbyInfo.account
            });
        }

        private void OnReceivedMatchState(IMatchState matchState)
        {

            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);

            switch (matchState.OpCode)
            {
                case OpCodeType.MATCH_SPAWN_PLAYER:
                    if (GameManager.Instance.IsHost) break;

                    SpawnPlayerNetworkEntity player = JsonParser.FromJson<SpawnPlayerNetworkEntity>(jsonUtf8);

                    AddPlayerRemotely(player);
                    break;
            }
        }

        public void Init()
        {
            GameManager.Instance.Connection.Socket.ReceivedMatchState += OnReceivedMatchState;
        }
    }
}