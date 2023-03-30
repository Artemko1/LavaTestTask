using Data;
using Infrastructure.SerializableDictionary;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "LootSpritesByType", menuName = "LootSpritesByType", order = 0)]
    public class LootSpritesByType : ScriptableObject
    {
        [SerializeField] private LootTypeSpriteDictionary _lootSprites;

        public Sprite GetForType(LootType lootType) => 
            _lootSprites[lootType];
    }
}