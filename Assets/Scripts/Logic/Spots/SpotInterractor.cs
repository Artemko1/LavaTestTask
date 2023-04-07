using System.Collections;
using System.Collections.Generic;
using Data;
using Data.DataLoot;
using DG.Tweening;
using Logic.Drop;
using Logic.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    [RequireComponent(typeof(Collider))]
    public class SpotInterractor : MonoBehaviour
    {
        private readonly List<Spot> _nearSpots = new List<Spot>();

        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;

        [SerializeField] private CharacterMovement _characterMovement;

        [SerializeField] private Transform _dropTransitionSpawnPoint;

        [SerializeField] private float _interactionCooldown = 1;

        private float _remainingCooldown;
        private int _transfersInProgressCount;
        private LootData _lootData;

        private void Start() =>
            _lootData = _playerProgressProvider.PlayerProgress.LootData;

        private void Update()
        {
            _remainingCooldown -= Time.deltaTime;
            if (Ready())
            {
                TryGiveLootToSpot();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var spot = other.GetComponent<Spot>();
            Assert.IsNotNull(spot);
            _nearSpots.Add(spot);
        }

        private void OnTriggerExit(Collider other)
        {
            var spot = other.GetComponent<Spot>();
            _nearSpots.Remove(spot);
        }

        private bool Ready() =>
            _characterMovement.IsMoving == false && _remainingCooldown < 0 && _nearSpots.Count > 0;

        private void TryGiveLootToSpot()
        {
            Spot spot = _nearSpots[0];

            // if (spot.RemainingRequiredLoot.Amount <= 0) return;
            Assert.IsTrue(_transfersInProgressCount >= 0);
            if (spot.RemainingRequiredLoot.Amount <= _transfersInProgressCount) return;

            Loot loot = _lootData.TrySubtractOne(spot.RemainingRequiredLoot.Type);

            if (loot == null || loot.Amount == 0) return;


            AnimateOneLootDrop(loot, spot);
            
            _remainingCooldown = _interactionCooldown;
        }

        private void AnimateOneLootDrop(Loot loot, Spot spot)
        {
            _transfersInProgressCount++;
            
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            DroppedLoot drop = Instantiate(droppedLootPrefab, _dropTransitionSpawnPoint.position, Random.rotation);
            var lootPerDrop = new Loot(loot.Type);
            drop.Init(lootPerDrop, false);

            Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.1f, 0.9f);
            Vector3 endMove = drop.transform.position + new Vector3(randomHorizontalMagnitude.x, 1, randomHorizontalMagnitude.y);

            DOTween.Sequence()
                .Append(drop.transform.DOMove(endMove, 0.3f))
                .Append(drop.transform.DOJump(spot.LootAcceptancePoint, 1.5f, 1, 0.3f))
                .Join(drop.transform.DOScale(0.5f, 0.3f))
                .OnComplete(() =>
                {
                    spot.Collect(lootPerDrop);
                    Destroy(drop.gameObject);
                    _transfersInProgressCount--;
                })
                .SetEase(Ease.Linear);
        }
    }
}