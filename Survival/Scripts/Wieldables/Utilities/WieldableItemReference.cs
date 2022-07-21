using UnityEngine;

namespace SurvivalTemplatePro.WieldableSystem
{
    [RequireComponent(typeof(Wieldable))]
    [AddComponentMenu("Wieldables/Utilities/Item Reference")]
    public sealed class WieldableItemReference : MonoBehaviour
    {
        public IWieldable Wieldable
        {
            get
            {
                if (m_Wieldable == null)
                    m_Wieldable = GetComponent<IWieldable>();

                return m_Wieldable;
            }
        }

        public ItemReference ItemReference => m_ItemReference;

        [SerializeField]
        private ItemReference m_ItemReference;

        private IWieldable m_Wieldable;
    }
}