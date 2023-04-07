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
    [RequireComponent(typeof(Collider), typeof(DropSpawner))]
    public class SpotInterractor : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;

        [SerializeField] private CharacterMovement _characterMovement;

        [SerializeField] private Transform _dropTransitionSpawnPoint;

        [SerializeField] private float _interactionCooldown = 1;

        private readonly List<Spot> _nearSpots = new List<Spot>();
        private DropSpawner _dropSpawner;
        private LootData _lootData;

        private float _remainingCooldown;
        private int _transfersInProgressCount;

        private void Awake() =>
            _dropSpawner = GetComponent<DropSpawner>();

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

            Assert.IsTrue(_transfersInProgressCount >= 0);
            if (spot.RemainingRequiredLoot.Amount <= _transfersInProgressCount) return;

            Loot loot = _lootData.TrySubtractOne(spot.RemainingRequiredLoot.Type);

            if (loot == null || loot.Amount == 0) return;

            AnimateOneLootTransfer(loot, spot);

            _remainingCooldown = _interactionCooldown;
        }

        private void AnimateOneLootTransfer(Loot loot, Spot spot)
        {
            _transfersInProgressCount++;

            DroppedLoot drop = _dropSpawner.SpawnNonCollectable(loot, _dropTransitionSpawnPoint.position, Random.rotation);

            Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.1f, 0.9f);
            Vector3 endMove = drop.transform.position + new Vector3(randomHorizontalMagnitude.x, 1, randomHorizontalMagnitude.y);

            DOTween.Sequence()
                .Append(drop.transform.DOMove(endMove, 0.3f))
                .Append(drop.transform.DOJump(spot.LootAcceptancePoint, 1.5f, 1, 0.3f))
                .Join(drop.transform.DOScale(0.5f, 0.3f))
                .OnComplete(() =>
                {
                    spot.Collect(loot);
                    Destroy(drop.gameObject);
                    _transfersInProgressCount--;
                })
                .SetEase(Ease.Linear);
        }
    }
}