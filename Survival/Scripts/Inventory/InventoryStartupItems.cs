using SurvivalTemplatePro.InventorySystem;
using UnityEngine;

namespace SurvivalTemplatePro
{
    [RequireComponent(typeof(IInventory))]
    [HelpURL("https://polymindgames.gitbook.io/welcome-to-gitbook/qgUktTCVlUDA7CAODZfe/player/modules-and-behaviours/inventory#inventory-startup-items-behaviour")]
    public class InventoryStartupItems : MonoBehaviour
    {
        [SerializeField]
        [InfoBox("A predefined list of items that will be added to the inventory.")]
        private InventoryStartupItemsInfo m_InventoryStartupInfo;


        private void Start() => AddStartupItems();

        public void AddStartupItems()
        {
            if (m_InventoryStartupInfo != null)
                m_InventoryStartupInfo.AddItemsToInventory(GetComponent<IInventory>());
        }
    }
}