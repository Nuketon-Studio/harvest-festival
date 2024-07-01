using HarvestFestival.SO;
using Nakama;
using UnityEngine;

namespace HarvestFestival.Entities.Network {
    class UserLobbyNetworkEntity: AutorityNetworkEntity
    {
        public GameObject instance;
        public IApiUser account;
        public CharacterSO characterStats;

        public bool isReady = false;
    }
}