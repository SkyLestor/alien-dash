using Scripts.Characters.Player;
using Scripts.RoundManagement;

namespace Scripts.GameEventBus
{
    public class GamePhaseChangedEvent
    {
        public GamePhase CurrentPhase;
    }

    public class PlayerDamagedEvent
    {
        public PlayerController Player;
    }

    public class ActivePlayersAmountChangedEvent
    {
        public int CurrentActivePlayersAmount;
    }
}