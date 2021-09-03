using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Items/Ingredient Item")]
    public class IngredientItem : Item
    {
        [Header("Ingredient Status")]
        public string kind;
        public int quantity;
        [Range(1, 5)] public int rarity = 1;

        private void OnValidate()
        {
            if (quantity < 0)
                quantity = 0;
        }
    }
}
