using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network
{
    [Serializable]
    class RotateNetworkEntity
    {
        public float x;
        public float y;
        public float z;

        public Quaternion toEuler() => Quaternion.Euler(x, y, z);
        public bool IsChange() => new Vector3(x, y, z) != Vector3.zero;
    }
}