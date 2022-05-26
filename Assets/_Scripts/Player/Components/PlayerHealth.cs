using System;
using System.Collections;
using UnityEngine;

namespace Root.Player.Components
{
    public class PlayerHealth : PlayerComponent
    {
        [SerializeField] private int initialMaxHealth = 4;
        [SerializeField] private float invincibilityTime = 0.3f;
        [Space]
        [SerializeField] private SpriteRenderer spriteRenderer;

        internal bool invincible;

        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value == maxHealth) return;

                maxHealth = value;

                if (maxHealth < 0)
                    maxHealth = 0;

                OnHealthChanged?.Invoke();
            }
        }

        private int currentHealth;
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (value == currentHealth) return;

                if (value > initialMaxHealth)
                {
                    value = initialMaxHealth;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                if (value < currentHealth)
                    OnPlayerDamaged?.Invoke();

                currentHealth = value;

                OnHealthChanged?.Invoke();

                if (value <= 0)
                    deathManager.CurrentDeathStage = DeathStages.Dying;
            }
        }

        public static event Action OnHealthChanged = delegate { };
        public static event Action OnPlayerDamaged = delegate { };

        private void Start()
        {
            PlayerDeathManager.OnDeathStageChanged += DeathEvents;

            MaxHealth = initialMaxHealth;
            CurrentHealth = MaxHealth;
        }

        private void OnDestroy() => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
                    StopAllCoroutines();
                    invincible = true;
                    break;
                case DeathStages.Resetting:
                    StopAllCoroutines();
                    ResetHealth();
                    break;
                case DeathStages.Done:
                    invincible = false;
                    break;
            }
        }

        public void Damage(int damage, Vector2 knockbackDirection = default, bool allowIFrames = true)
        {
            if (invincible) return;

            ApplyDamage(damage, allowIFrames);

            if (knockbackDirection == default) return;

            knockback.Knockback(knockbackDirection);
        }

        public void Damage(int damage, Vector2 knockbackDirection, float knockbackForce, float knockbackTime, bool allowIFrames = true)
        {
            if (invincible) return;

            ApplyDamage(damage, allowIFrames);
            knockback.Knockback(knockbackDirection, knockbackForce, knockbackTime);
        }

        private void ApplyDamage(int damage, bool allowIFrames = true)
        {
            Instantiate(particleEffects.damaged, center.position, Quaternion.identity);

            CurrentHealth -= damage;

            if (!allowIFrames || CurrentHealth <= 0) return;

            StartCoroutine(InvincibilityFrames());
        }

        private IEnumerator InvincibilityFrames()
        {
            invincible = true;

            float invincibilityTimer = invincibilityTime;

            bool colorSwitch = false;

            while (invincibilityTimer > 0)
            {
                invincibilityTimer -= Time.deltaTime;

                colorSwitch = !colorSwitch;

                spriteRenderer.color = colorSwitch ? Color.clear : Color.white;

                yield return null;
            }

            spriteRenderer.color = Color.white;

            invincible = false;
        }

        public void ResetHealth()
        {
            invincible = false;
            CurrentHealth = MaxHealth;
        }
    }
}