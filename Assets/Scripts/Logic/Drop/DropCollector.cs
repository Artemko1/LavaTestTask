using UnityEngine;

namespace Logic.Drop
{
    [RequireComponent(typeof(Collider))]
    public class DropCollector : MonoBehaviour
    {
        [SerializeField] private Transform _collectorMagnetTransform;

        private void OnTriggerEnter(Collider other) =>
            TryCollect(other);

        private void OnTriggerStay(Collider other) =>
            TryCollect(other);

        private void TryCollect(Component other)
        {
            var droppedLoot = other.GetComponent<DroppedLoot>();
            if (droppedLoot.CanBeCollected)
            {
                droppedLoot.Collect(_collectorMagnetTransform.transform.position);
            }
        }
    }
}