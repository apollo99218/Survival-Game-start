using System;
using UnityEngine;

namespace SurvivalTemplatePro.InventorySystem
{
    [Serializable]
    public class ItemGenerator
    {
        [SerializeField]
        private ItemGenerationMethod Method;

        [SerializeField]
        private ItemReference SpecificItem;

        [Range(1, 30)]
        [SerializeField]
        private int Count = 1;

        [SerializeField]
        private ItemCategoryReference Category;


        public Item GenerateItem() 
        {
            if (Method == ItemGenerationMethod.Specific)
            {
                return new Item(SpecificItem.GetItem(), Count);
            }
            else if (Method == ItemGenerationMethod.RandomFromCategory)
            {
                ItemInfo itemInfo = ItemDatabase.GetRandomItemFromCategory(Category);

                if (itemInfo != null)
                    return new Item(itemInfo, Count);
            }
            else if (Method == ItemGenerationMethod.Random)
            {
                var category = ItemDatabase.GetRandomCategory();

                if (category != null)
                {
                    ItemInfo itemInfo = ItemDatabase.GetRandomItemFromCategory(category.Name);

                    if (itemInfo != null)
                        return new Item(itemInfo, Count);
                }
            }

            return null;
        }

        public ItemInfo GetItemInfo() 
        {
            if (Method == ItemGenerationMethod.Specific)
            {
                return SpecificItem.GetItem();
            }
            else if (Method == ItemGenerationMethod.RandomFromCategory)
            {
                ItemInfo itemInfo = ItemDatabase.GetRandomItemFromCategory(Category);
                return itemInfo;
            }
            else if (Method == ItemGenerationMethod.Random)
            {
                var category = ItemDatabase.GetRandomCategory();

                if (category != null)
                {
                    ItemInfo itemInfo = ItemDatabase.GetRandomItemFromCategory(category.Name);
                    return itemInfo;
                }
            }

            return null;
        }
    }

    public enum ItemGenerationMethod { Specific, Random, RandomFromCategory }
}