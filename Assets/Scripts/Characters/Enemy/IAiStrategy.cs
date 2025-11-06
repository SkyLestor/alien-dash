using System.Collections;

namespace Scripts.Characters.Enemy
{
    public interface IAiStrategy
    {
        IEnumerator InitializeMovementStrategy(EnemyController controller);
    }
}