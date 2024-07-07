using DG.Tweening;
using HarvestFestival.Effects;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Watermelon : Projectile
    {
        [SerializeField] private GameObject hitSplashEffectPrefab;
        [SerializeField] private ProjectileSO stats;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.parent.TryGetComponent(out Character character))
            {
                character.Hit(stats.damage);

                Destroy(gameObject);
                return;
            }

            Destroy(gameObject, 3f);
        }


        public override void Init(GameObject shooter, Vector3 direction)
        {
            transform.position = shooter.transform.Find("CameraSpot/AIM").transform.position + shooter.transform.forward * .5f;

            Vector3 directionForce = direction * stats.force;

            GetComponent<Rigidbody>().AddForce(directionForce, ForceMode.Impulse);
        }
    }
}