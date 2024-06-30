using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class SpawnPlayerNetworkEntity
    {
        public float x;
        public float y;
        public float z;

        public string userId;

        public Vector3 toVector3() => new Vector3(x, y, z);
    }
}