using Nakama;
using UnityEngine;

namespace HarvestFestival.Entities.Network {
    class UserNetworkEntity: AutorityNetworkEntity
    {
        public Character character;
        public IApiUser account;

        public bool isDead;
        public bool isLocal;  
    }
}