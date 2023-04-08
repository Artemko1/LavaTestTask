using DG.Tweening;
using UnityEngine;

namespace Logic.Spots
{
    [CreateAssetMenu(fileName = "LootToSpotTransferSettings", menuName = "Spot/LootToSpotTransferSettings", order = 0)]
    public class LootToSpotTransferSettings : ScriptableObject
    {
        [field: SerializeField] public float FloatingDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float JumpAndScaleDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float JumpPower { get; private set; } = 1.5f;
        [field: SerializeField] public float ScaleEndValue { get; private set; } = 0.5f;
        [field: SerializeField] public float HorizontalMagnitudeMin { get; private set; } = 0f;
        [field: SerializeField] public float HorizontalMagnitudeMax { get; private set; } = 1f;
        [field: SerializeField] public float FloatingDistance { get; private set; } = 1f;
        [field: SerializeField] public Ease SequenceEase { get; private set; } = Ease.Linear;
    }
}