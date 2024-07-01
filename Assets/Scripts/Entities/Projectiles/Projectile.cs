using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public static void Fire(GameObject shooter, Vector3 direction, string namePrefab)
        {
            var prefab = Resources.Load<GameObject>(namePrefab);

            if (prefab != null)
            {
                var instance = Instantiate(prefab);

                instance.GetComponent<Projectile>().Init(shooter, direction);
            }
        }

        public virtual void Init(GameObject shooter, Vector3 direction) { }
    }
}