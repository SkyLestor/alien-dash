using DG.Tweening;
using Scripts.Characters.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Characters.Enemy.EnemyDrops
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyDrop : MonoBehaviour, IPoolable<Vector3, int, EnemyDropsPool>
    {
        [SerializeField] [Tooltip("How far the drop scatters on spawn")]
        private float _scatterRadius = 1.5f;

        [SerializeField] [Tooltip("How long the scatter animation takes")]
        private float _scatterDuration = 0.3f;

        [SerializeField] [Tooltip("How fast does it fly to the player")]
        private float _flyingSpeed = 8;

        [SerializeField] [Tooltip("How close the drop needs to be to get picked up")]
        private float _pickupDistance = 0.1f;
        

        private Collider2D _collider2D;
        private EnemyDropsPool _pool;

        private PlayerController _target;
        private int _value;
        private Vector3 _initialScale;
        private float _pickupDistanceSqr;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _initialScale = transform.localScale;
            _pickupDistanceSqr = _pickupDistance * _pickupDistance;
        }

        private void Update()
        {
            if (!_target)
            {
                return;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                _target.transform.position,
                _flyingSpeed * Time.deltaTime);
            float sqrDistance = (transform.position - _target.transform.position).sqrMagnitude;
            if (sqrDistance < _pickupDistanceSqr)
            {
                ApplyEffect(_target);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_target)
            {
                return;
            }

            if (!other.TryGetComponent<PlayerController>(out var player))
            {
                return;
            }

            _collider2D.enabled = false;
            _target = player;
        }

        public void OnDespawned()
        {
            _target = null;
            _pool = null;
        }

        public void OnSpawned(Vector3 position, int value, EnemyDropsPool pool)
        {
            _value = value;
            _pool = pool;

            _collider2D.enabled = true;
            transform.DOKill();
            transform.position = position;
            transform.localScale = _initialScale;

            Vector2 startPos = transform.position;
            var randomOffset = Random.insideUnitCircle * _scatterRadius;

            transform.DOMove(startPos + randomOffset, _scatterDuration)
                .SetEase(Ease.OutBack);
        }

        private void ApplyEffect(PlayerController player)
        {
            if (!player)
            {
                return;
            }
            player.Data.AddExperience(_value);
            _pool.Despawn(this);
        }
    }

    public class EnemyDropsPool : MonoMemoryPool<Vector3, int, EnemyDrop>
    {
        protected override void Reinitialize(Vector3 position, int value, EnemyDrop item)
        {
            item.OnSpawned(position, value, this);
        }

        protected override void OnDespawned(EnemyDrop item)
        {
            item.OnDespawned();
            base.OnDespawned(item);
        }
    }
}