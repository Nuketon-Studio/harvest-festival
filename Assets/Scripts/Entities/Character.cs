using HarvestFestival.Controllers;
using HarvestFestival.Helpers;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Entities
{
    class Character : MonoBehaviour
    {
        [SerializeField] protected PlayerController playerController;
        [SerializeField] protected CharacterSO stats;

        [Header("Debug")]
        [SerializeField]protected bool _isOffline = false;

        protected int _hp;
        protected string _userId;
        protected Skin _skin;

        #region Actions
        public virtual void Hit(int damage) { }
        #endregion

        public virtual void Init(CharacterSO character, string userId)
        {
            // transform.position = new Vector3(Random.Range(-5, 5), 3, Random.Range(-5, 5));
            stats = character;
            _userId = userId;

            playerController?.Init(stats);
            _hp = stats.hp;

            playerController = GetComponent<PlayerController>();
        }

        protected virtual void Start()
        {
            var instance = Instantiate(PrefabHelper.Load(stats.skin));
            instance.transform.position = transform.position;
            instance.transform.SetParent(transform);

            _skin = instance.GetComponent<Skin>();
        }
    }
}