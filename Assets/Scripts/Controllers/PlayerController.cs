using HarvestFestival.Entities.Network;
using HarvestFestival.Entities.Projectiles;
using HarvestFestival.Helpers;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.Controllers
{
    class PlayerController : MonoBehaviour
    {
        private bool _canMove = true;
        private PositionNetworkEntity _position;
        private CharacterSO _character;

        public void Init(CharacterSO character)
        {
            _character = character;
        }

        #region Gets/Sets
        public void SetCanMove(bool value) => _canMove = value;
        #endregion

        #region Actions
        public void Move(PositionNetworkEntity position)
        {
            _position = position;

            if (!_canMove || _position.toVector3() == Vector3.zero) return;

            transform.Translate(_position.toVector3() * _character.speed * Time.deltaTime);
        }

        public void Jump()
        {
            if (!_canMove) return;

            transform.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * _character.speed, ForceMode.Impulse);
        }

        public void Attack(Vector3 direction, string prefabName)
        {
            var projectile = PrefabHelper.Load(prefabName);
            Projectile.Create(gameObject, direction, projectile);
        }
        #endregion
    }
}