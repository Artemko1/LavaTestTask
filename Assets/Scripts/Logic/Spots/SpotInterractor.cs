using System.Collections.Generic;
using Data;
using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    [RequireComponent(typeof(Collider))]
    public class SpotInterractor : MonoBehaviour
    {
        private readonly List<Spot> _nearSpots = new List<Spot>();

        [SerializeField] private PlayerProgressProvider _playerProgressProvider;


        [SerializeField] private float _interactionCooldown = 5;
        private float _remainingCooldown;
        private LootData _lootData;

        private void Start() =>
            _lootData = _playerProgressProvider.PlayerProgress.LootData;

        private void Update() =>
            _remainingCooldown -= Time.deltaTime;

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

        public void TryGiveLootToSpot()
        {
            if (!Ready()) return;

            Spot spot = _nearSpots[0];

            Loot loot = _lootData.TrySubtract(spot.RemainingRequiredLoot.Type, spot.RemainingRequiredLoot.Amount);

            if (loot == null || loot.Amount == 0) return;

            spot.Collect(loot);
            _remainingCooldown = _interactionCooldown;
        }

        private bool Ready() =>
            _remainingCooldown < 0 && _nearSpots.Count > 0;
    }
}