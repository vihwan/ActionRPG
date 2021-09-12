using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    [System.Serializable]
    public class PrefabDataBase : MonoBehaviour
    {
        [Header("Prefab Setting")]
        public ShopListSlot shopListSlot;
        public RewardItemSlot rewardItemSlot;
        public EnforceItemSlot enforceItemSlot;

        [Header("Rarity Materials")]
        public List<Material> materials = new List<Material>();

        public void Init()
        {
            shopListSlot = Resources.Load<ShopListSlot>("Prefabs/ShopSlots/ShopListSlot");
        }
    }
}
