using Data.DataLoot;
using UnityEngine;

namespace Logic.Drop
{
    public class DropSpawner : MonoBehaviour
    {
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;

        public void SpawnWithBurst(Loot loot, Vector3 position)
        {
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            DroppedLoot drop = Instantiate(droppedLootPrefab, position, Random.rotation);
            drop.Init(loot, true);

            Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.75f, 1.25f);
            float verticalMagnitude = Random.Range(4f, 5f);

            drop.AddRigidbodyForce(new Vector3(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y));
        }
    }
}