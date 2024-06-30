using Nakama;
using UnityEngine;

namespace HarvestFestival.Entities.Network {
    class UserNetworkEntity
    {
        public Character character;
        public IApiUser account;
        public string userId;

        public bool isDead;
        public bool isLocal;  
    }
}