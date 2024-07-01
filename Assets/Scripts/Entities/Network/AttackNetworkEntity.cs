using System;
using UnityEngine;

namespace HarvestFestival.Entities.Network {
    [Serializable]
    class AttackNetworkEntity: AutorityNetworkEntity
    {
        public string prefabName;

        public float x;
        public float y;
        public float z;

        public Vector3 toVector3() => new Vector3(x, y, z);
    }
}