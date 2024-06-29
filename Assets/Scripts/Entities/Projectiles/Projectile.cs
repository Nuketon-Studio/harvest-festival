using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Projectile : MonoBehaviour
    {
        public static void Fire(GameObject shooter, string namePrefab)
        {
            var prefab = Resources.Load<GameObject>("Projectiles/MelonPrefab");

            if (prefab != null)
            {
                var instance = Instantiate(prefab);

                instance.GetComponent<Projectile>().Init(shooter);
            }
        }

        public virtual void Init(GameObject shooter) { }
    }
}