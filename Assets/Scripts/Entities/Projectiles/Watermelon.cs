using DG.Tweening;
using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Watermelon : Projectile
    {
        public float force = 1f;
        public int damage = 50;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Character character))
            {
                character.Hit(damage);
            }

            Destroy(gameObject, 3f);
        }

        public override void Init(GameObject shooter, Vector3 direction)
        {
            transform.position = shooter.transform.Find("CameraSpot/AIM").transform.position + shooter.transform.forward * .5f;

            Vector3 directionForce = direction * force;

            GetComponent<Rigidbody>().AddForce(directionForce, ForceMode.Impulse);
        }
    }
}