using UnityEditor;

namespace Infrastructure.SerializableDictionary.Editor
{
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(LootTypeSpriteDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}
