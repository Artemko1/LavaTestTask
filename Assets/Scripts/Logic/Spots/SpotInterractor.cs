using System;
using System.Collections.Generic;
using Data;
using Data.DataLoot;
using Logic.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    [RequireComponent(typeof(Collider), typeof(LootToSpotTransferer))]
    public class SpotInterractor : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private CharacterMovement _characterMovement;

        [SerializeField] private SpotInterractorSettings _settings;

        private readonly List<Spot> _nearSpots = new List<Spot>();

        private LootToSpotTransferer _lootToSpotTransferer;

        private float _remainingCooldown;
        private int _transfersInProgressCount;

        private void Awake()
        {
            _lootToSpotTransferer = GetComponent<LootToSpotTransferer>();
            Assert.IsNotNull(_playerProgressProvider);
            Assert.IsNotNull(_characterMovement);
            Assert.IsNotNull(_settings);
        }

        private void OnEnable()
        {
            var capsule = GetComponent<CapsuleCollider>();
            capsule.radius = _settings.InteractionRadius;
        }

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

            Loot loot = _playerProgressProvider.PlayerProgress.LootData.TrySubtractOne(spot.RemainingRequiredLoot.Type);

            if (loot == null || loot.Amount == 0) return;

            AnimateOneLootTransfer(loot, spot);

            ResetCooldown();
        }

        private void AnimateOneLootTransfer(Loot loot, Spot spot)
        {
            _transfersInProgressCount++;
            _lootToSpotTransferer.Transfer(loot, spot, () => _transfersInProgressCount--);
        }

        private void ResetCooldown() =>
            _remainingCooldown = _settings.InteractionCooldown;
    }
}