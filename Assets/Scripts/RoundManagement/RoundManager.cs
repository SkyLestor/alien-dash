using System.Collections;
using Scripts.GameEventBus;
using UnityEngine;

namespace Scripts.RoundManagement
{
    public class RoundManager : MonoBehaviour, IRoundManager
    {
        private const int RoundDuration = 40;
        private GamePhase _currentPhase;

        private void Awake()
        {
            CurrentPhase = GamePhase.Play;
            StartCoroutine(Round());
        }

        private void OnEnable()
        {
            EventBus.Subscribe<ActivePlayersAmountChangedEvent>(OnActivePlayersAmountChangedEvent);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ActivePlayersAmountChangedEvent>(OnActivePlayersAmountChangedEvent);
        }

        public float RoundTime { get; private set; }

        public GamePhase CurrentPhase
        {
            get => _currentPhase;
            private set
            {
                _currentPhase = value;
                EventBus.Raise(new GamePhaseChangedEvent { CurrentPhase = _currentPhase });
            }
        }

        private void OnActivePlayersAmountChangedEvent(ActivePlayersAmountChangedEvent eventData)
        {
            if (eventData.CurrentActivePlayersAmount == 0)
            {
                CurrentPhase = GamePhase.Finish;
            }
        }

        public IEnumerator Round()
        {
            RoundTime = RoundDuration;

            while (RoundTime > 0)
            {
                RoundTime -= Time.deltaTime;
                yield return null;
            }

            CurrentPhase = GamePhase.Finish;
        }
    }
}