using UnityEngine;

namespace Logic.Drop
{
    [CreateAssetMenu(fileName = "DroppedLootSettings", menuName = "Drop/DroppedLootSettings", order = 0)]
    public class DroppedLootSettings : ScriptableObject
    {
        [field: SerializeField] public float DelayBeforeCollection { get; private set; } = 1f;
        [field: SerializeField] public float GravityForce { get; private set; } = -6f;
    }
}