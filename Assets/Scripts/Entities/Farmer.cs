using System;
using UnityEngine;

namespace HarvestFestival.Entities
{
    public class Farmer : MonoBehaviour
    {
        public Action<GameObject> OnCollisionEnterCallback;

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterCallback(other.gameObject);
        }

        public void Talk() {
            Debug.Log("Opa acho que rolou agora emmmm");
        }
    }
}