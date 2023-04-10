using DG.Tweening;
using UnityEngine;

namespace Logic.Drop
{
    [CreateAssetMenu(fileName = "DropCollectorSettings", menuName = "Drop/DropCollectorSettings", order = 0)]
    public class DropCollectorSettings : ScriptableObject
    {
        [field: SerializeField] public float CollectRadius { get; private set; } = 1.25f;
        [field: SerializeField] public float JumpPower { get; private set; } = 0.5f;
        [field: SerializeField] public float CollectionDuration { get; private set; } = 0.35f;
        [field: SerializeField] public float ScaleEndValue { get; private set; } = 0.1f;
        [field: SerializeField] public Ease AnimationEase { get; private set; } = Ease.Linear;
    }
}