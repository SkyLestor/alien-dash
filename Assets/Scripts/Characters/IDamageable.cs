namespace Scripts.Characters
{
    public interface IDamageable
    {
        int CurrentHeath { get; }
        int MaxHealth { get; }
        void TakeDamage(int damage);
    }
}