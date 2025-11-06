using Scripts.GameEventBus;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.RoundManagement.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RoundTimer : MonoBehaviour
    {
        private const float UpdatePeriod = 10;
        private IRoundManager _roundManager;
        private float _timer;
        private TextMeshProUGUI _timerText;


        [Inject]
        public void Construct(IRoundManager roundManager)
        {
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _timerText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _timer += 1;
            if (!(_timer >= UpdatePeriod))
            {
                return;
            }

            _timer = 0;
            _timerText.text = $"{Mathf.Ceil(_roundManager.RoundTime)}";
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);
        }

        private void OnGamePhaseChanged(GamePhaseChangedEvent eventData)
        {
            if (eventData.CurrentPhase == GamePhase.Finish)
            {
                Destroy(gameObject);
            }
        }
    }
}