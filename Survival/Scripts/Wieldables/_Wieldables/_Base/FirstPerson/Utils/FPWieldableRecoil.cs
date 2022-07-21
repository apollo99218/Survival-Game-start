using System.Collections.Generic;
using UnityEngine;

namespace SurvivalTemplatePro.WieldableSystem
{
    [AddComponentMenu("Wieldables/First Person/FP Recoil")]
    [RequireComponent(typeof(FPWieldableMotion))]
    public class FPWieldableRecoil : STPWieldableEventsListener<FPWieldableRecoil>
    {
        #region Internal
        [System.Serializable]
        protected class RecoilEventSettings : STPEventListenerBehaviour
        {
            public RecoilType RecoilType;

            [Space]

            public FPRecoilForce Force;
            public ShakeSettings Shake;


            protected override void OnActionTriggered(float recoilMod)
            {
                switch (RecoilType)
                {
                    case RecoilType.Force:
                        Instance.AddRecoilForce(Force, recoilMod);
                        break;
                    case RecoilType.Skake:
                        Instance.DoShake(Shake, recoilMod);
                        break;
                    case RecoilType.Both:
                        Instance.AddRecoilForce(Force, recoilMod);
                        Instance.DoShake(Shake, recoilMod);
                        break;
                }
            }
        }

        protected enum RecoilType
        {
            Force,
            Skake,
            Both
        }
        #endregion

        [SerializeField, Range(0f, 100f)]
        private float m_SpringLerpSpeed = 30f;

        [Space]

        [SerializeField]
        private Spring.Settings m_PositionSpring = Spring.Settings.Default;

        [SerializeField]
        private Spring.Settings m_RotationSpring = Spring.Settings.Default;

        [Space]

        [SerializeField]
        private RecoilEventSettings[] m_RecoilSettings;

        private Spring m_PosSpring;
        private Spring m_RotSpring;

        private readonly List<SpringShake> m_Shakes = new List<SpringShake>();


        #region Public Methods
        public void AddRecoilForce(FPRecoilForce recoilForce, float recoilMod = 1f)
        {
            m_PosSpring.AddForce(recoilForce.GetPositionForce(recoilMod));
            m_RotSpring.AddForce(recoilForce.GetRotationForce(recoilMod));
        }

        public void AddPositionRecoil(SpringForce force) => m_PosSpring.AddForce(force);
        public void AddRotationRecoil(SpringForce force) => m_RotSpring.AddForce(force);

        public void SetCustomSpringSettings(Spring.Settings positionSettings, Spring.Settings rotationSettings)
        {
            m_PosSpring.Adjust(positionSettings);
            m_RotSpring.Adjust(rotationSettings);
        }

        public void ClearCustomSpringSettings() 
        {
            m_PosSpring.Adjust(m_PositionSpring);
            m_RotSpring.Adjust(m_RotationSpring);
        }

        public void DoShake(ShakeSettings shake, float shakeScale = 1f) => m_Shakes.Add(new SpringShake(shake, m_PosSpring, m_RotSpring, shakeScale));
        #endregion

        protected override IEnumerable<STPEventListenerBehaviour> GetEvents() => m_RecoilSettings;

        protected override void Awake()
        {
            base.Awake();

            Transform pivot = GetComponent<FPWieldableMotion>().Pivot;

            m_PosSpring = new Spring(Spring.RefreshType.AddToPosition, pivot, Vector3.zero, m_SpringLerpSpeed);
            m_RotSpring = new Spring(Spring.RefreshType.AddToRotation, pivot, Vector3.zero, m_SpringLerpSpeed);

            SetCustomSpringSettings(m_PositionSpring, m_RotationSpring);
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            m_PosSpring.FixedUpdate(fixedDeltaTime);
            m_RotSpring.FixedUpdate(fixedDeltaTime);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            UpdateShakes();

            m_PosSpring.Update(deltaTime);
            m_RotSpring.Update(deltaTime);
        }

        private void UpdateShakes()
        {
            if (m_Shakes.Count == 0)
                return;

            int i = 0;

            while (true)
            {
                if (m_Shakes[i].IsDone)
                    m_Shakes.RemoveAt(i);
                else
                {
                    m_Shakes[i].Update();
                    i++;
                }

                if (i >= m_Shakes.Count)
                    break;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && m_PosSpring != null)
            {
                m_PosSpring.Adjust(m_PositionSpring);
                m_RotSpring.Adjust(m_RotationSpring);
            }
        }
#endif
    }
}