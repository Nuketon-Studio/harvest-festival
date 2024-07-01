using System;
using HarvestFestival.SO;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class UpdateCharacterNetworkEntity : AutorityNetworkEntity
    {
        public string characterName;
    }
}