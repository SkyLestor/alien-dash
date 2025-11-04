namespace Scripts.Characters.Player
{
    public interface IAnimationsController
    {
        void PlayIdleAnimation();
        void PlayRunAnimation();
        void StartDashAnimation();
        void FinishDashAnimation();
        void PlayDamagedAnimation();
    }
}