using Data.DataLoot;
using Infrastructure.SerializableDictionary;
using UnityEngine;

namespace Logic.Drop
{
    [CreateAssetMenu(fileName = "LootDropPrefabsByType", menuName = "Loot/LootDropPrefabsByType", order = 1)]
    public class LootDropPrefabsByType : ScriptableObject
    {
        [SerializeField] private LootTypeDroppedLootDictionary _lootPrefabs;

        public DroppedLoot GetForType(LootType lootType) =>
            _lootPrefabs[lootType];
    }
}