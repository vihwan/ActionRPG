using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Items/Ingredient Item")]
    public class IngredientItem : Item
    {
        private void OnValidate()
        {
            if (itemType != ItemType.Ingredient)
                itemType = ItemType.Ingredient;

            if (quantity < 0)
                quantity = 0;
        }
    }
}
