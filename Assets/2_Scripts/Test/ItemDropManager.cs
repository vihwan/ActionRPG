using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ItemDropManager : MonoBehaviour
    {
        public static ItemDropManager Instance;

        [Header("DropItem Prefabs")]
        [SerializeField] private DropItemBox dropItemPrefab;

        [Header("Rarity Material")]
        public Material rare1_material;
        public Material rare2_material;
        public Material rare3_material;
        public Material rare4_material;
        public Material rare5_material;
        private void Awake()
        {
            Instance = this;

            dropItemPrefab = Resources.Load<DropItemBox>("Prefabs/DropItemBox");

            rare1_material = Database.Instance.prefabDatabase.materials[0];
            rare2_material = Resources.Load<Material>("Prefabs/Material/DropItemRarity/star2");
            rare3_material = Resources.Load<Material>("Prefabs/Material/DropItemRarity/star3");
            rare4_material = Resources.Load<Material>("Prefabs/Material/DropItemRarity/star4");
            rare5_material = Resources.Load<Material>("Prefabs/Material/DropItemRarity/star5");
        }
        public void GenerateDropItemBox(List<Item> items, Transform dropTransfom)
        {
            DropItemBox newDropItem = Instantiate(dropItemPrefab, GetRandomPosition(dropTransfom.position), Quaternion.identity);
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
