using HarvestFestival.Entities.Projectiles;
using UnityEngine;

namespace HarvestFestival.SO
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/new character", order = 0)]
    public class CharacterSO : ScriptableObject
    {
        public string displayName = "";
        public int hp;
        public float speed;
        public float speedRun;

        public string projectile;
    }
}