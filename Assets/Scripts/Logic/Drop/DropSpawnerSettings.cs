using UnityEngine;

namespace Logic.Drop
{
    [CreateAssetMenu(fileName = "DropSpawnerSettings", menuName = "Drop/DropSpawnerSettings", order = 0)]
    public class DropSpawnerSettings : ScriptableObject
    {
        [field: SerializeField] public float HorizontalMagnitudeMin { get; private set; } = 0.75f;
        [field: SerializeField] public float HorizontalMagnitudeMax { get; private set; } = 1.25f;
        [field: SerializeField] public float VerticalMagnitudeMin { get; private set; } = 4.0f;
        [field: SerializeField] public float VerticalMagnitudeMax { get; private set; } = 5.0f;
    }
}