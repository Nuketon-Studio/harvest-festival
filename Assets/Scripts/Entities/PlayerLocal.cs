using System.Threading.Tasks;
using HarvestFestival.Controllers;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.SO;
using HarvestFestival.Types;
using UnityEngine;

namespace HarvestFestival.Entities
{
    [RequireComponent(typeof(PlayerController))]
    class PlayerLocal : Character
    {
        [Header("Debug")]
        [SerializeField] private bool isOffline = false;

        [SerializeField] private Animator animator;

        private PositionNetworkEntity _currentPosition = new PositionNetworkEntity { };
        private Quaternion _lastRotate;
        private bool _isInputMove = false;
        private bool _isFalling = false;
        private GameObject _skin;

        public override void Init(CharacterSO character, string userId)
        {
            base.Init(character, userId);

            var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
            if (camera) camera.Attachment(gameObject.transform.Find("CameraSpot").transform);
        }

        #region Actions
        private void Move()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            if (horizontal == 0 && vertical == 0)
            {
                _isInputMove = false;
                animator.SetBool("Walk", false);
                return;
            }

            _currentPosition = new PositionNetworkEntity
            {
                x = horizontal,
                y = 0,
                z = vertical,
                userId = GameManager.Instance?.UserId
            };

            animator.SetBool("Walk", true);
            playerController.Move(_currentPosition);
            _isInputMove = true;
        }

        private async void Jump()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || _isFalling) return;

            _isFalling = true;
            animator.SetTrigger("Jump");
            await Task.Delay(900);
            playerController.Jump();
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack");

                Invoke(nameof(Fire), .8f);
            }
        }

        public async void Fire()
        {
            var direction = Camera.main.transform.forward;

            playerController.Attack(direction, stats.projectile);

            if (!isOffline)
                await NetworkHelper.Send<AttackNetworkEntity>(OpCodeType.PLAYER_ATTACK_LIGHT, new AttackNetworkEntity
                {
                    userId = GameManager.Instance.UserId,
                    prefabName = stats.projectile,
                    x = direction.x,
                    y = direction.y,
                    z = direction.z,
                });
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

        #region Callbacks Events
        private void OnCollisionEnterCallback(GameObject other)
        {
            if (_isFalling)
            {
                animator.SetBool("Falling", false);
                _isFalling = false;
            }
        }
        #endregion

        #region Unity Events
        void Start()
        {
            _skin = Instantiate(stats.skin);
            _skin.transform.SetParent(transform);

            animator = _skin.GetComponent<Animator>();

            _skin.GetComponent<Farmer>().OnCollisionEnterCallback += OnCollisionEnterCallback;

            // esse metodo é só para poder testar sem ta conectado
            // quando abrir a cena direto p testar mecanica e tals no player
            if (!isOffline) return;

            var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
            if (camera) camera.Attachment(gameObject.transform);

            Init(stats, "");
        }

        void Update()
        {
            Move();
            Attack();
            Jump();
        }

        private async void LateUpdate()
        {
            if (isOffline) return;

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
        #endregion
    }
}