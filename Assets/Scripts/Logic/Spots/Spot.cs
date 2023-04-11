using System;
using System.Collections;
using Data.DataLoot;
using Logic.Drop;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    [SelectionBase, RequireComponent(typeof(DropSpawner))]
    public class Spot : MonoBehaviour
    {
        [SerializeField] private Transform _lootAcceptancePoint;
        [SerializeField] private SpotSettings _settings;

        private DropSpawner _dropSpawner;

        public Action RefineStart;

        public Loot RemainingRequiredLoot { get; private set; }
        public Vector3 LootAcceptancePoint => _lootAcceptancePoint.position;

        private void Awake()
        {
            Assert.IsNotNull(_settings);
            Assert.IsFalse(_settings.TotalRequiredLoot.Type == LootType.None);
            Assert.IsFalse(_settings.TotalRequiredLoot.Amount <= 0);
            Assert.IsFalse(_settings.RefinedLoot.Type == LootType.None);
            Assert.IsFalse(_settings.RefinedLoot.Amount <= 0);
            Assert.IsFalse(_settings.RefineTime <= 0);

            _dropSpawner = GetComponent<DropSpawner>();

            ResetRemainingRequiredLoot();
        }

        public void Collect(Loot loot)
        {
            Assert.AreEqual(_settings.TotalRequiredLoot.Type, loot.Type, "Trying to collect wrong loot");
            Assert.IsFalse(loot.Amount > RemainingRequiredLoot.Amount, "Too many loot is passed to the spot");

            RemainingRequiredLoot.Amount -= loot.Amount;

            if (RemainingRequiredLoot.Amount == 0)
            {
                StartCoroutine(RefineLoot());
            }
        }

        private IEnumerator RefineLoot()
        {
            RefineStart?.Invoke();

            float oneStepTime = _settings.RefineTime / _settings.RefinedLoot.Amount;

            for (var i = 0; i < _settings.RefinedLoot.Amount; i++)
            {
                yield return new WaitForSeconds(oneStepTime);

                _dropSpawner.SpawnWithBurst(new Loot(_settings.RefinedLoot.Type, 1), _lootAcceptancePoint.position);
            }

            yield return new WaitForSeconds(oneStepTime);

            ResetRemainingRequiredLoot();
        }

        private void ResetRemainingRequiredLoot() =>
            RemainingRequiredLoot = _settings.TotalRequiredLoot.Clone();
    }
}