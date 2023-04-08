using UnityEngine;

namespace Logic.Spots
{
    [CreateAssetMenu(fileName = "SpotInterractorSettings", menuName = "Spot/SpotInterractorSettings", order = 0)]
    public class SpotInterractorSettings : ScriptableObject
    {
        [field: SerializeField] public float InteractionCooldown { get; private set; } = 0.15f;
    }
}