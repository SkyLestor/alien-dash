using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Characters.Enemy;
using Scripts.GameEventBus;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.RoundManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 0.5f;
        [SerializeField] private int _enemiesPerWave = 30;
        private readonly List<EnemyController> _activeEnemies = new();
        private BoxCollider2D _boxCollider2D;

        private DiContainer _container;
        private EnemiesContainerSo _enemiesContainerSo;

        private Coroutine _spawnCoroutine;

        [Inject]
        public void Construct(EnemiesContainerSo enemiesContainerSo, DiContainer container)
        {
            _enemiesContainerSo = enemiesContainerSo;
            _container = container;
        }

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            StartSpawning();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);
            StopSpawning();
        }

        private void OnGamePhaseChanged(GamePhaseChangedEvent eventData)
        {
            if (eventData.CurrentPhase == GamePhase.Play)
            {
                StartSpawning();
            }
            else if (eventData.CurrentPhase is GamePhase.Finish or GamePhase.Upgrade)
            {
                StopSpawning();
                _activeEnemies.Clear();
            }
        }

        private void StartSpawning()
        {
            if (_spawnCoroutine == null)
            {
                _spawnCoroutine = StartCoroutine(SpawnWaveCoroutine());
            }
        }

        private void StopSpawning()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnWaveCoroutine()
        {
            var enemiesSpawned = 0;

            var availableTypes = _enemiesContainerSo.EnemyPrefabs.Keys.ToList();

            while (enemiesSpawned < _enemiesPerWave)
            {
                var typeToSpawn = availableTypes[Random.Range(0, availableTypes.Count)];

                var pools = _container.ResolveIdAll<EnemyPool>(typeToSpawn);

                if (pools == null || pools.Count == 0)
                {
                    Debug.LogError($"No enemy pools found for type: {typeToSpawn}. Did you set up the EnemyInstaller?");
                    yield return new WaitForSeconds(_spawnInterval);
                    continue;
                }

                var poolToUse = pools[Random.Range(0, pools.Count)];

                var spawnPos = GetRandomSpawnPoint();

                var enemy = poolToUse.Spawn(spawnPos, Quaternion.identity, poolToUse);

                _activeEnemies.Add(enemy);
                enemiesSpawned++;

                yield return new WaitForSeconds(_spawnInterval);
            }

            _spawnCoroutine = null;
        }

        private Vector2 GetRandomSpawnPoint()
        {
            var bounds = _boxCollider2D.bounds;
            return new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }
    }
}