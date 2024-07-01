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
        protected string _userId;
        public virtual void Hit(int damage) { }

        public virtual void Init(CharacterSO character, string userId)
        {
            transform.position = new Vector3(Random.Range(-5, 5), 3, Random.Range(-5, 5));
            stats = character;
            _userId = userId;

            playerController?.Init(stats);
            _hp = stats.hp;

            playerController = GetComponent<PlayerController>();
        }
    }
}