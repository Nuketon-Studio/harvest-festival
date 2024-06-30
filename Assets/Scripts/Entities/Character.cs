using HarvestFestival.Controllers;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    class Character : MonoBehaviour
    {
        [SerializeField] protected PlayerController playerController;
        [SerializeField] protected CharacterSO stats;

        protected int _hp;
        public virtual void Hit(int damage) { }

        public void Init(CharacterSO character)
        {
            transform.position = new Vector3(Random.Range(-5, 5), 3, Random.Range(-5, 5));
            stats = character;

            playerController?.Init(stats);
            _hp = stats.hp;

            playerController = GetComponent<PlayerController>();
        }
    }
}