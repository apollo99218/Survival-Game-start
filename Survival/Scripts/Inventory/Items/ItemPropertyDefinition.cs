using System;
using UnityEngine;

namespace SurvivalTemplatePro.InventorySystem
{
    [Serializable]
    public class ItemPropertyDefinition : ICloneable
    {
        [HideInInspector]
        public int Id;

        public string Name;

        public ItemPropertyType Type;


        public object Clone() => (ItemPropertyDefinition)MemberwiseClone();
    }
}