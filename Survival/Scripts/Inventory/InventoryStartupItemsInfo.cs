using System;
using UnityEngine;

namespace SurvivalTemplatePro.InventorySystem
{
    [CreateAssetMenu(menuName = "Survival Template Pro/Inventory/Startup Items")]
    public class InventoryStartupItemsInfo : ScriptableObject
    {
        #region Internal
        [Serializable]
        public class ItemContainerStartupItems
        {
            public string Name;
            public ItemGenerator[] StartupItems;
        }
        #endregion

        [SerializeField]
        private ItemContainerStartupItems[] m_ItemContainersStartupItems;


        public void AddItemsToInventory(IInventory inventory)
        {
            foreach (var container in m_ItemContainersStartupItems)
            {
                ItemContainer itemContainer = inventory.GetContainerWithName(container.Name);

                if (itemContainer == null)
                    continue;

                foreach (var itemGenerator in container.StartupItems)
                    itemContainer.AddItem(itemGenerator.GenerateItem());
            }
        }
    }
}