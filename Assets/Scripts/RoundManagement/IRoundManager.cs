namespace Scripts.RoundManagement
{
    public interface IRoundManager
    {
        float RoundTime { get; }
        GamePhase CurrentPhase { get; }
    }
}