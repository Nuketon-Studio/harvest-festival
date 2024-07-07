using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public static void Create(GameObject shooter, Vector3 direction, GameObject prefab)
        {
            if (prefab != null)
            {
                var instance = Instantiate(prefab);

                instance.GetComponent<Projectile>().Init(shooter, direction);
            }
        }

        public virtual void Init(GameObject shooter, Vector3 direction) { }
    }
}