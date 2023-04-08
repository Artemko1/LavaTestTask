using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Logic.Drop
{
    public class DropSpawner : MonoBehaviour
    {
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;
        [SerializeField] private DropSpawnerSettings _settings;

        private void Awake()
        {
            Assert.IsNotNull(_lootDropPrefabsByType);
            Assert.IsNotNull(_settings);
        }

        public void SpawnWithBurst(Loot loot, Vector3 position)
        {
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            DroppedLoot drop = Instantiate(droppedLootPrefab, position, Random.rotation);
            drop.Init(loot, true);

            Vector2 randomHorizontalMagnitude =
                Random.insideUnitCircle * Random.Range(_settings.HorizontalMagnitudeMin, _settings.HorizontalMagnitudeMax);
            float verticalMagnitude = Random.Range(_settings.VerticalMagnitudeMin, _settings.VerticalMagnitudeMax);

            drop.AddRigidbodyForce(new Vector3(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y));
        }
    }
}