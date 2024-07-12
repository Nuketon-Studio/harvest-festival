using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.SO;
using HarvestFestival.States;
using HarvestFestival.Types;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerLocal : Character
    {
        private PositionNetworkEntity _currentPosition = new PositionNetworkEntity { };
        private PlayerStateMachine _state;
        private Quaternion _lastRotate;
        private bool _isInputMove = false;
        private bool _isFalling = false;

        public override void Init(CharacterSO character, string userId)
        {
            base.Init(character, userId);

            // attachment camera
            var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
            if (camera) camera.Attachment(gameObject.transform.Find("CameraSpot").transform);

            _state = new PlayerStateMachine();
            _state.OnChangeState += _skin.OnChangeState;

            _skin.OnCollisionEnterCallback += OnSkinCollisionEnter;
        }

        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var speed = stats.speed;

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                speed = stats.speedRun;
            }

            if (horizontal == 0 && vertical == 0)
            {
                _isInputMove = false;
                _state?.SetState(PlayerStateType.Walk_Stop);
                return;
            }

            _currentPosition = new PositionNetworkEntity
            {
                x = horizontal,
                y = 0,
                z = vertical,
                speed = speed,
                userId = GameManager.Instance?.UserId
            };

            _state.SetState(PlayerStateType.Walk);

            playerController.Move(_currentPosition);
            _isInputMove = true;
        }

        private void Jump()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || _isFalling) return;

            _isFalling = true;
            _state.SetState(PlayerStateType.Jump);

            _skin.WaitMomentAnimationToJump(() => playerController.Jump());
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var direction = Camera.main.transform.forward;
                _state.SetState(PlayerStateType.Attack);

                _skin.WaitMomentAnimationToAttack(async () =>
                {
                    playerController.Attack(direction, stats.projectile);

                    if (!_isOffline)
                        await NetworkHelper.Send<AttackNetworkEntity>(OpCodeType.PLAYER_ATTACK_LIGHT, new AttackNetworkEntity
                        {
                            userId = GameManager.Instance.UserId,
                            prefabName = stats.projectile,
                            x = direction.x,
                            y = direction.y,
                            z = direction.z,
                        });
                });
            }
        }

        public override void Hit(int damage)
        {
            _hp -= damage;

            if (_hp <= 0) Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Collisions
        private void OnSkinCollisionEnter(Collision other)
        {
            if(other.transform.CompareTag("Ground") && _isFalling) _isFalling = false;
        }
        #endregion

        #region Unity Events
        protected override void Start()
        {
            base.Start();

            // esse metodo é só para poder testar sem ta conectado
            // quando abrir a cena direto p testar mecanica e tals no player
            if (_isOffline)
            {
                Init(stats, "");
            }
        }

        void Update()
        {
            Move();
            Attack();
            Jump();
        }

        private async void LateUpdate()
        {
            if (_isOffline) return;

            if (_currentPosition is not null && _isInputMove && _currentPosition.toVector3() != Vector3.zero)
                await NetworkHelper.Send<PositionNetworkEntity>(OpCodeType.PLAYER_MOVE, _currentPosition);

            if (_lastRotate != transform.rotation)
            {
                var rotate = new RotateNetworkEntity
                {
                    x = transform.eulerAngles.x,
                    y = transform.eulerAngles.y,
                    z = transform.eulerAngles.z,
                };

                if (rotate.IsChange()) await NetworkHelper.Send<RotateNetworkEntity>(OpCodeType.PLAYER_ROTATE, rotate);

                _lastRotate = transform.rotation;
            }

        }

        void OnDestroy()
        {
            _state.OnChangeState -= _skin.OnChangeState;
        }
        #endregion
    }
}