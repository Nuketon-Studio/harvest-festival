using UnityEngine;

namespace HarvestFestival.Entities.Projectiles
{
    class Projectile : MonoBehaviour
    {
        public static void Fire() {
            var prefab = Resources.Load<GameObject>("Projectiles/MelonPrefab");

            if(prefab != null) {
                var instance = Instantiate(prefab);

                instance.GetComponent<Projectile>().Init();                
            }
        }

        public virtual void Init() { }
    }
}