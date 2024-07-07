using UnityEngine;

namespace HarvestFestival.SO
{
    [CreateAssetMenu(fileName = "ProjectileSO", menuName = "ScriptableObjects/New Projectile", order = 0)]
    public class ProjectileSO : ScriptableObject
    {
        public string displayName;
        public int damage;
        public float force = 30f;
    }
}