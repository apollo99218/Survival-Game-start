using SurvivalTemplatePro.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalTemplatePro.UISystem
{
    public class CraftingUI : PlayerUIBehaviour
    {
        [SerializeField]
        private PanelUI m_CraftingPanel;

        [Space]

        [SerializeField]
        private CraftingSlotUI m_Template;

        [SerializeField]
        private RectTransform m_TemplateSpawnRect;

        [SerializeField, Range(5, 20)]
        private int m_TemplateInstanceCount = 10;

        private List<CraftingSlotUI> m_CachedSlots = new List<CraftingSlotUI>();
        private ICraftingManager m_CraftingManager;


        public override void OnAttachment()
        {
            GetModule(out m_CraftingManager);

            m_CraftingPanel.onToggled += OnPanelToggled;

            InitializeCraftingSlots();
        }

        public override void OnDetachment()
        {
            m_CraftingPanel.onToggled -= OnPanelToggled;
        }

        private void OnPanelToggled(bool show)
        {
            if (show)
            {
                Player.Inventory.onContainerChanged += UpdateCraftRequirments;
                UpdateCraftRequirments(null);
            }
            else
            {
                Player.Inventory.onContainerChanged -= UpdateCraftRequirments;
                m_CraftingManager.CancelCrafting();
            }
        }

        private void InitializeCraftingSlots() 
        {
            int remainingSlots = m_TemplateInstanceCount;

            foreach (var category in ItemDatabase.GetAllCategories())
            {
                foreach (var item in category.Items)
                {
                    if (remainingSlots == 0)
                        break;

                    if (item.Crafting.IsCraftable)
                    {
                        m_CachedSlots.Add(Instantiate(m_Template.gameObject, m_TemplateSpawnRect).GetComponent<CraftingSlotUI>());

                        int currentIndex = m_TemplateInstanceCount - remainingSlots;
                        m_CachedSlots[currentIndex].onClick += StartCrafting;
                        m_CachedSlots[currentIndex].DisplayItem(item);

                        remainingSlots--;
                    }
                }
            }
        }

        private void UpdateCraftRequirments(ItemSlot slot)
        {
            for (int i = 0; i < m_CachedSlots.Count; i++)
                m_CachedSlots[i].UpdateRequirementsUI(Player.Inventory);
        }

        private void StartCrafting(ItemInfo itemInfo) => m_CraftingManager.Craft(itemInfo);
    }
}