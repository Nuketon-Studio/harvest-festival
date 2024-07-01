using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class RotateNetworkEntity: AutorityNetworkEntity
    {
        public float x;
        public float y;
        public float z;

        public Vector3 toVector3() => new Vector3(x, y, z);
        public bool IsChange() => new Vector3(x, y, z) != Vector3.zero;
    }
}