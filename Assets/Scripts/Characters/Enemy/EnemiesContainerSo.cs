using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Scripts.Characters.Enemy
{
    [CreateAssetMenu(menuName = "Scriptable/EnemiesContainerSo", fileName = "EnemiesContainerSo")]
    public class EnemiesContainerSo : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<EnemyType, EnemyController[]> _enemyPrefabs;

        public SerializedDictionary<EnemyType, EnemyController[]> EnemyPrefabs => _enemyPrefabs;
    }
}