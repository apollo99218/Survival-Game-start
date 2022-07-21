using SurvivalTemplatePro.InventorySystem;
using System.Collections;
using UnityEngine;

namespace SurvivalTemplatePro.BuildingSystem
{
    [HelpURL("https://polymindgames.gitbook.io/welcome-to-gitbook/qgUktTCVlUDA7CAODZfe/interaction/interactable/demo-interactables")]
    public class StorageCrate : Interactable, IExternalContainer
    {
        public ItemContainer ItemContainer => m_ItemContainer;

        [BHeader("Settings (Storage Crate)")]

        [SerializeField]
        [Tooltip("How many slots should this storage crate have.")]
        private int m_StorageSpots;

        [BHeader("Animation")]

        [SerializeField]
        [Tooltip("Crate cover transform (used for the open/close animation")]
        private Transform m_Cover;

        [SerializeField]
        [Tooltip("Should the cover be animated?")]
        private bool m_AnimateCover;

        [SerializeField]
        [Tooltip("How long should the open/close animations last.")]
        private float m_AnimationDuration = 1f;

        [SerializeField]
        [Tooltip("Animation easing type.")]
        private Easings.Function m_AnimationStyle = Easings.Function.QuadraticEaseInOut;

        [SerializeField]
        [Tooltip("The crate cover closed rotation")]
        private Vector3 m_ClosedRotation;

        [SerializeField]
        [Tooltip("The crate cover open rotation.")]
        private Vector3 m_OpenRotation;

        [SerializeField]
        private Vector3 m_ClosedPosition;

        [SerializeField]
        private Vector3 m_OpenPosition;

        [Space]

        [SerializeField]
        private ItemGenerator[] m_InitialItems;

        private Easer m_CoverEaser;
        private ItemContainer m_ItemContainer;


        public void OpenCrate()
        {
            if (m_AnimateCover)
            {
                StopAllCoroutines();
                StartCoroutine(C_OpenCrate(true));
            }
        }

        public void CloseCrate()
        {
            if (m_AnimateCover)
            {
                StopAllCoroutines();
                StartCoroutine(C_OpenCrate(false));
            }
        }

        private void Awake()
        {
            InitializeCrateCover();
            GenerateContainer();
        }

        private void InitializeCrateCover() 
        {
            m_CoverEaser = new Easer(Easings.Function.QuadraticEaseInOut, m_AnimationDuration);

            m_Cover.localPosition = m_ClosedPosition;
            m_Cover.localRotation = Quaternion.Euler(m_ClosedRotation);
        }

        private void GenerateContainer()
        {
            m_ItemContainer = new ItemContainer("Storage", 100, m_StorageSpots, ItemContainerFlags.External, null, null, null);

            foreach (var itemGenerator in m_InitialItems)
                m_ItemContainer.AddItem(itemGenerator.GenerateItem());
        }

        private IEnumerator C_OpenCrate(bool open)
        {
            m_CoverEaser.Reset();
            m_CoverEaser.Duration = m_AnimationDuration;
            m_CoverEaser.Function = m_AnimationStyle;

            Quaternion startRotation = m_Cover.localRotation;
            Quaternion targetRotation = Quaternion.Euler(open ? m_OpenRotation : m_ClosedRotation);

            Vector3 startPosition = m_Cover.localPosition;
            Vector3 targetPosition = open ? m_OpenPosition : m_ClosedPosition;

            while (m_CoverEaser.InterpolatedValue < 1f)
            {
                m_CoverEaser.Update(Time.deltaTime);
                m_Cover.localRotation = Quaternion.Lerp(startRotation, targetRotation, m_CoverEaser.InterpolatedValue);
                m_Cover.localPosition = Vector3.Lerp(startPosition, targetPosition, m_CoverEaser.InterpolatedValue);

                yield return null;
            }
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (m_CoverEaser != null)
            {
                m_CoverEaser.Function = m_AnimationStyle;
                m_CoverEaser.Duration = m_AnimationDuration;
            }
        }
        #endif
    }
}