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
        public DropItemBox dropItemBox;

        [Header("Rarity Materials")]
        public List<Material> materials = new List<Material>();

        public void Init()
        {
            shopListSlot = Resources.Load<ShopListSlot>("Prefabs/ShopSlots/ShopListSlot");
            dropItemBox = Resources.Load<DropItemBox>("Prefabs/DropItemBox");
        }

        public Material GetMaterialByName(string name)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].name.Equals(name))
                {
                    return materials[i];
                }
            }

            return null;
        }
    }
}
