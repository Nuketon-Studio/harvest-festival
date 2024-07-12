using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class PositionNetworkEntity: AutorityNetworkEntity
    {
        public float x;
        public float y;
        public float z;
        public float speed;

        public Vector3 toVector3() => new Vector3(x, y, z);
    }
}