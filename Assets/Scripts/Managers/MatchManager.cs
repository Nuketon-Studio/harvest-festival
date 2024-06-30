using System.Collections.Generic;
using HarvestFestival.Entities;
using HarvestFestival.Entities.Network;
using UnityEngine;

namespace HarvestFestival.Managers
{
    class MatchManager : MonoBehaviour
    {
        [field: SerializeField] public List<UserNetworkEntity> players { get; private set; } = new List<UserNetworkEntity>();

        [SerializeField] private GameObject playerRemotePrefab;
        [SerializeField] private GameObject playerLocalPrefab;

        public string MatchId { get; private set; }

        public void SetMatchId(string matchId) => MatchId = matchId;

        public void AddPlayers(List<UserLobbyNetworkEntity> usersLobby)
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
            }
        }
    }
}