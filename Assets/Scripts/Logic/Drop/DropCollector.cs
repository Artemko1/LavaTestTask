using System;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Logic.Drop
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class DropCollector : MonoBehaviour
    {
        [SerializeField] private Transform _collectorMagnetTransform;
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private DropCollectorSettings _settings;

        private void OnEnable()
        {
            var capsule = GetComponent<CapsuleCollider>();
            capsule.radius = _settings.CollectRadius;
        }

        private void OnTriggerEnter(Collider other) =>
            TryCollect(other);

        private void OnTriggerStay(Collider other) =>
            TryCollect(other);

        private void TryCollect(Component other)
        {
            var drop = other.GetComponent<DroppedLoot>();
            if (!drop.CanBeCollected) return;

            drop.MarkCollected();

            DOTween.Sequence()
                .Append(drop.transform.DOJump(_collectorMagnetTransform.transform.position, _settings.JumpPower, 1,
                    _settings.CollectionDuration))
                .Join(drop.transform.DOScale(_settings.ScaleEndValue, _settings.CollectionDuration))
                .OnComplete(() =>
                {
                    _playerProgressProvider.PlayerProgress.LootData.Collect(drop.Loot);
                    Destroy(drop.gameObject);
                })
                .SetEase(_settings.AnimationEase);
        }
    }
}