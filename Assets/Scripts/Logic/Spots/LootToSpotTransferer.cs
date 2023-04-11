using System;
using Data.DataLoot;
using DG.Tweening;
using Logic.Drop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Spots
{
    public class LootToSpotTransferer : MonoBehaviour
    {
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;
        [SerializeField] private Transform _dropTransitionSpawnPoint;
        [SerializeField] private LootToSpotTransferSettings _transferSettings;

        public void Transfer(Loot loot, Spot spot, Action onComplete = null)
        {
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            DroppedLoot drop = Instantiate(droppedLootPrefab, _dropTransitionSpawnPoint.position, Random.rotation);
            drop.Init(loot, false);


            float random = Random.Range(_transferSettings.HorizontalMagnitudeMin,
                _transferSettings.HorizontalMagnitudeMax);
            Vector2 randomHorizMagnitude = Random.insideUnitCircle * random;
            Vector3 endMove = drop.transform.position +
                              new Vector3(randomHorizMagnitude.x, _transferSettings.FloatingUpDistance,
                                  randomHorizMagnitude.y);

            DOTween.Sequence()
                .Append(drop.transform.DOMove(endMove, _transferSettings.FloatingDuration))
                .Append(drop.transform.DOJump(spot.LootAcceptancePoint, _transferSettings.JumpPower, 1,
                    _transferSettings.JumpAndScaleDuration))
                .Join(drop.transform.DOScale(_transferSettings.ScaleEndValue, _transferSettings.JumpAndScaleDuration))
                .OnComplete(() =>
                {
                    spot.Collect(loot);
                    Destroy(drop.gameObject);
                    onComplete?.Invoke();
                })
                .SetEase(_transferSettings.SequenceEase);
        }
    }
}