using HarvestFestival.SO;
using Nakama;
using UnityEngine;

namespace HarvestFestival.Entities.Network {
    class UserLobbyNetworkEntity
    {
        public GameObject instance;
        public IApiUser account;
        public CharacterSO characterStats;

        public string userId;
        public bool isReady = false;
    }
}