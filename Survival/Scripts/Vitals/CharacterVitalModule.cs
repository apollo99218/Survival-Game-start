using UnityEngine;
using UnityEngine.Events;

namespace SurvivalTemplatePro
{
    [RequireComponent(typeof(IHealthManager))]
    public abstract class CharacterVitalModule : MonoBehaviour
    {
        #region Internal
        [System.Serializable]
        protected class StatSettings
        {
            [Range(0f, 1000f)]
            public float InitialValue = 100f;

            [Range(0f, 1000f)]
            public float InitialMaxValue = 100f;

            [Space]

            public DepletionType DepletionType = DepletionType.DecreaseValue;

            [Range(0f, 100f)]
            public float DepletionSpeed = 3f;

            [Space]

            [InfoBox("After the stat value croses this threshold the character will start taking damage.")]
            public ValueThresholdType DamageThresholdType = ValueThresholdType.SmallerThan;

            [Range(0f, 1000f)]
            public float DamageValueThreshold = 5f;

            [Range(0f, 100f)]
            public float Damage = 3f;

            [Space]

            [InfoBox("After the stat value croses this threshold the character's health will start to restore")]
            public ValueThresholdType HealThresoldType = ValueThresholdType.BiggerThan;

            [Range(0f, 1000f)]
            public float HealValueThreshold = 95f;

            [Range(0f, 100f)]
            public float HealthRestore = 3f;

            [Space]

            public UnityEvent m_OnDepleted;
        }

        protected enum DepletionType
        {
            IncreaseValue,
            DecreaseValue,
        }

        protected enum ValueThresholdType
        {
            BiggerThan,
            SmallerThan,
            None
        }
        #endregion

        [SerializeField]
        protected StatSettings m_StatSettings;

        protected IHealthManager m_HealthManager;


        protected virtual void Awake()
        {
            if (!TryGetComponent(out m_HealthManager))
            {
                Debug.LogError($"This component requires a component that implements the 'IVitalsManager' module to function, Disabling... ");
                enabled = false;
            }
        }

        protected void InitalizeStat(ref float statValue, ref float maxStatValue)
        {
            statValue = Mathf.Clamp(m_StatSettings.InitialValue, 0f, m_StatSettings.InitialMaxValue);
            maxStatValue = m_StatSettings.InitialMaxValue;
        }

        protected void DepleteStat(ref float statValue, float maxStatValue) 
        {
            float deltaTime = Time.deltaTime;
            float depletion = (m_StatSettings.DepletionType == DepletionType.IncreaseValue) ? (m_StatSettings.DepletionSpeed * deltaTime) : -(m_StatSettings.DepletionSpeed * deltaTime);

            statValue = Mathf.Clamp(statValue + depletion, 0, maxStatValue);

            // Apply damage
            switch (m_StatSettings.DamageThresholdType)
            {
                case ValueThresholdType.BiggerThan:
                    if (statValue > m_StatSettings.DamageValueThreshold)
                        m_HealthManager.ReceiveDamage(m_StatSettings.Damage * deltaTime);
                    break;
                case ValueThresholdType.SmallerThan:
                    if (statValue < m_StatSettings.DamageValueThreshold)
                        m_HealthManager.ReceiveDamage(m_StatSettings.Damage * deltaTime);
                    break;
                case ValueThresholdType.None:
                    break;
            }

            // Restore health
            switch (m_StatSettings.HealThresoldType)
            {
                case ValueThresholdType.BiggerThan:
                    if (statValue > m_StatSettings.HealValueThreshold)
                        m_HealthManager.RestoreHealth(m_StatSettings.HealthRestore * deltaTime);
                    break;
                case ValueThresholdType.SmallerThan:
                    if (statValue < m_StatSettings.HealValueThreshold)
                        m_HealthManager.RestoreHealth(m_StatSettings.HealthRestore * deltaTime);
                    break;
                case ValueThresholdType.None:
                    break;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() 
        {
            if (m_StatSettings == null)
                return;

            m_StatSettings.InitialValue = Mathf.Clamp(m_StatSettings.InitialValue, 0f, m_StatSettings.InitialMaxValue);
        }
#endif
    }
}