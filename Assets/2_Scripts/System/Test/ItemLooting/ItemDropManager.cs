using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ItemDropManager : MonoBehaviourSingleton<ItemDropManager>
    {
        [Header("DropItem Prefabs")]
        [SerializeField] private DropItemBox dropItemBoxPrefab;

        [Header("Rarity Material")]
        public Material rare1_material;
        public Material rare2_material;
        public Material rare3_material;
        public Material rare4_material;
        public Material rare5_material;
        public void Init()
        {
            dropItemBoxPrefab = Database.it.PrefabDataBase.dropItemBox;

            rare1_material = Database.it.PrefabDataBase.GetMaterialByName("star1");
            rare2_material = Database.it.PrefabDataBase.GetMaterialByName("star2");
            rare3_material = Database.it.PrefabDataBase.GetMaterialByName("star3");
            rare4_material = Database.it.PrefabDataBase.GetMaterialByName("star4");
            rare5_material = Database.it.PrefabDataBase.GetMaterialByName("star5");
        }
        public void GenerateDropItemBox(List<Item> items, Transform dropTransfom)
        {
            DropItemBox newDropItem = Instantiate(dropItemBoxPrefab, GetRandomPosition(dropTransfom.position), Quaternion.identity);
            newDropItem.SetItem(items);
            newDropItem.GetComponentInChildren<MeshRenderer>().material = SetDropItemRarity(items);
        }

        private Vector3 GetRandomPosition(Vector3 position)
        {
            float x = Random.Range(0, 0.5f);
            float y = 1f;
            float z = Random.Range(0, 0.5f);

            position += new Vector3(x, y, z);

            return position;
        }

        private Material SetDropItemRarity(List<Item> items)
        {
            int rarity = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if(rarity < items[i].rarity)
                {
                    rarity = items[i].rarity;
                }
            }

            switch (rarity)
            {
                case 1: return rare1_material;
                case 2: return rare2_material;
                case 3: return rare3_material;
                case 4: return rare4_material;
                case 5: return rare5_material;
                default: return rare1_material;
            }
        }
    }
}
