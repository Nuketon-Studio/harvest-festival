using UnityEngine;
using UnityEditor;

namespace HarvestFestival.SO
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/New Character", order = 0)]
    public class CharacterSO : ScriptableObject
    {
        public string displayName = "";
        public int hp;
        public float speed;
        public float speedRun;
        public float jumpForce;

        [HideInInspector]
        public string projectile;
        [HideInInspector]
        public int projectileIndex;

        [HideInInspector]
        public string skin;
        [HideInInspector]
        public int skinIndex;

    }
}