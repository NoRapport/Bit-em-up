using TopDownCharacter2D.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownCharacter2D.Health
{
    /// <summary>
    ///     Handles the health of an entity
    /// </summary>
    public class HealthSystem : MonoBehaviour
    {
        [Tooltip("The delay between two health changes in seconds")]
        [SerializeField] private float healthChangeDelay = .5f;

        [SerializeField] private UnityEvent onDamage;
        [SerializeField] private UnityEvent onHeal;
        [SerializeField] private UnityEvent onDeath;
        [SerializeField] private UnityEvent onInvincibilityEnd;

        private CharacterStatsHandler _statsHandler;
        private float _timeSinceLastChange = float.MaxValue;

        public UnityEvent OnDamage => onDamage;
        public UnityEvent OnHeal => onHeal;
        public UnityEvent OnDeath => onDeath;
        public UnityEvent OnInvincibilityEnd => onInvincibilityEnd;

        public float CurrentHealth { get; private set; }

        public float MaxHealth => _statsHandler.CurrentStats.maxHealth;

        private void Awake()
        {
            _statsHandler = GetComponent<CharacterStatsHandler>();
        }

        private void Start()
        {
            CurrentHealth = _statsHandler.CurrentStats.maxHealth;
        }

        private void Update()
        {
            if (_timeSinceLastChange < healthChangeDelay)
            {
                _timeSinceLastChange += Time.deltaTime;
                if (_timeSinceLastChange >= healthChangeDelay)
                {
                    onInvincibilityEnd.Invoke();
                }
            }
        }

        /// <summary>
        ///     Modifies the health of the entity
        /// </summary>
        /// <param name="change"> The amount of health to add</param>
        /// <returns></returns>
        public bool ChangeHealth(float change)
        {
            if (change == 0 || _timeSinceLastChange < healthChangeDelay)
            {
                return false;
            }

            _timeSinceLastChange = 0f;
            CurrentHealth += change;
            CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
            CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

            if (change > 0)
            {
                onHeal.Invoke();
            }
            else
            {
                OnDamage.Invoke();
            }

            if (CurrentHealth <= 0f)
            {
                Death();
            }

            return true;
        }

        private void Death()
        {
            onDeath.Invoke();
        }
    }
}