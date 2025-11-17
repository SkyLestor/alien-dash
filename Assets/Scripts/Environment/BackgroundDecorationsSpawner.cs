using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BackgroundDecorationsSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject[] _decorationPrefabs;

        [SerializeField] private int _amountToSpawn = 50;

        private BoxCollider2D _spawnArea;

        private void Awake()
        {
            _spawnArea = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (_decorationPrefabs.Length == 0)
            {
                Debug.LogWarning("No decoration prefabs found");
                return;
            }

            SpawnDecorations();
        }

        private void SpawnDecorations()
        {
            var bounds = _spawnArea.bounds;
            var propsParent = new GameObject("PropsParent").transform;
            for (var i = 0; i < _amountToSpawn; i++)
            {
                var randomX = Random.Range(bounds.min.x, bounds.max.x);
                var randomY = Random.Range(bounds.min.y, bounds.max.y);
                var spawnPosition = new Vector2(randomX, randomY);

                var prefabToSpawn = _decorationPrefabs[Random.Range(0, _decorationPrefabs.Length)];

                var prop = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, propsParent);
                prop.name = $"Prop_{i}";
            }
        }
    }
}