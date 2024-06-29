using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Melon : Projectile
    {

        public float distance = 10;

        private void OnCollisionEnter(Collision other)
        {
            Destroy(gameObject, 3f);
        }

        public override void Init()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance)) {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 10f);
                transform.position = hit.point;
            }
        }
    }
}