using UnityEditor;

namespace Infrastructure.SerializableDictionary.Editor
{
    [CustomPropertyDrawer(typeof(StringStringDictionary)), CustomPropertyDrawer(typeof(LootTypeSpriteDictionary)),
     CustomPropertyDrawer(typeof(LootTypeDroppedLootDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
    {
    }
}