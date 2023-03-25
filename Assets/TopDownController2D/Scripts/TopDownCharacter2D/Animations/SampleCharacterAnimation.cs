using TopDownCharacter2D.Health;
using UnityEngine;

namespace TopDownController2D.Scripts.TopDownCharacter2D.Animations
{
    public class SampleCharacterAnimation : TopDownAnimations
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        
        [SerializeField] private bool createDustOnWalk = true;
        [SerializeField] private ParticleSystem dustParticleSystem;
        
        private HealthSystem _healthSystem;
        
        protected override void Awake()
        {
            base.Awake();
            _healthSystem = GetComponent<HealthSystem>();
        }

        protected void Start()
        {
            controller.OnAttackEvent.AddListener(_ => Attacking());
            controller.OnMoveEvent.AddListener(Move);

            if (_healthSystem != null)
            {
                _healthSystem.OnDamage.AddListener(Hurt);
                _healthSystem.OnInvincibilityEnd.AddListener(InvincibilityEnd);
            }
        }

        /// <summary>
        ///     To call when the character moves, change the animation to the walking one
        /// </summary>
        /// <param name="movementDirection"> The new movement direction </param>
        private void Move(Vector2 movementDirection)
        {
            animator.SetBool(IsWalking, movementDirection.magnitude > .5f);
        }

        /// <summary>
        ///     To call when the character attack
        /// </summary>
        private void Attacking()
        {
            animator.SetTrigger(Attack);
        }

        /// <summary>
        ///     To call when the character takes damage
        /// </summary>
        private void Hurt()
        {
            animator.SetBool(IsHurt, true);
        }

        /// <summary>
        ///     To call when the character ends its invincibility time
        /// </summary>
        public void InvincibilityEnd()
        {
            animator.SetBool(IsHurt, false);
        }

        /// <summary>
        ///     Creates dust particles when the character walks, called from an animation
        /// </summary>
        public void CreateDustParticles()
        {
            if (createDustOnWalk)
            {
                dustParticleSystem.Stop();
                dustParticleSystem.Play();
            }
        }
    }
}