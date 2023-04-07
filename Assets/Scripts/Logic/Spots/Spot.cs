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
        [SerializeField] private Loot _totalRequiredLoot;

        [SerializeField] private Transform _lootAcceptancePoint;

        [SerializeField] private Loot _refinedLoot;
        [SerializeField] private float _refineTotalTime;

        private DropSpawner _dropSpawner;

        public Loot RemainingRequiredLoot { get; private set; }

        public Vector3 LootAcceptancePoint => _lootAcceptancePoint.position;

        private void Awake()
        {
            Assert.IsFalse(_totalRequiredLoot.Type == LootType.None);
            Assert.IsFalse(_totalRequiredLoot.Amount <= 0);
            Assert.IsFalse(_refinedLoot.Type == LootType.None);
            Assert.IsFalse(_refinedLoot.Amount <= 0);
            Assert.IsFalse(_refineTotalTime <= 0);

            _dropSpawner = GetComponent<DropSpawner>();

            ResetRemainingRequiredLoot();

        }

        public void Collect(Loot loot)
        {
            Assert.AreEqual(_totalRequiredLoot.Type, loot.Type, "Trying to collect wrong loot");
            Assert.IsFalse(loot.Amount > RemainingRequiredLoot.Amount, "Too many loot is passed to the spot");

            RemainingRequiredLoot.Amount -= loot.Amount;

            if (RemainingRequiredLoot.Amount == 0)
            {
                StartCoroutine(RefineLoot());
            }
        }

        private IEnumerator RefineLoot()
        {
            float oneStepTime = _refineTotalTime / _refinedLoot.Amount;

            for (var i = 0; i < _refinedLoot.Amount; i++)
            {
                yield return new WaitForSeconds(oneStepTime);

                _dropSpawner.SpawnCollectableWithBurst(new Loot(_refinedLoot.Type, 1), _lootAcceptancePoint.position, Random.rotation);
            }

            yield return new WaitForSeconds(oneStepTime);

            ResetRemainingRequiredLoot();
        }

        private void ResetRemainingRequiredLoot() =>
            RemainingRequiredLoot = _totalRequiredLoot.Clone();
    }
}