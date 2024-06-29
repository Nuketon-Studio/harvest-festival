using DG.Tweening;
using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Watermelon : Projectile
    {

        public float distance = 10;
        public float timeToDownProjectile = 1f;
        public int damage = 50;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Character character))
            {
                character.Hit(damage);
            }

            Destroy(gameObject, 3f);
        }

        public override void Init(GameObject shooter)
        {
            transform.position = shooter.transform.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float distanceShoot;

            if (Physics.Raycast(ray, out hit, distance))
            {
                distanceShoot = Vector3.Distance(hit.point, shooter.transform.position);

                Debug.DrawLine(shooter.transform.position, hit.point, Color.red, 10f);
                transform.DOJump(hit.point, distanceShoot / 2, 1, distanceShoot / timeToDownProjectile);
                return;
            }

            distanceShoot = Vector3.Distance(Camera.main.transform.TransformDirection(Vector3.forward) * distance, shooter.transform.position) / 2;
            transform.DOJump(Camera.main.transform.TransformDirection(Vector3.forward) * distance, distanceShoot / 2, 1, distanceShoot / timeToDownProjectile);
        }
    }
}