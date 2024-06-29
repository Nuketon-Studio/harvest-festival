using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class PositionEntityNetwork
    {
        public float x;
        public float y;
        public float z;

        public Vector3 toVector3() => new Vector3(x, y, z);
    }
}