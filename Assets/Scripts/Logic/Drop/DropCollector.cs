using UnityEngine;

namespace Logic.Drop
{
    public class DropCollector : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _dropObserver;

        [SerializeField] private Transform _collectorMagnetTransform;

        private void OnEnable()
        {
            _dropObserver.TriggerEnter += DropObserverOnTriggerEnter;
            _dropObserver.TriggerStay += DropObserverOnTriggerStay;
        }

        private void OnDisable()
        {
            _dropObserver.TriggerEnter -= DropObserverOnTriggerEnter;
            _dropObserver.TriggerStay -= DropObserverOnTriggerStay;
        }

        private void DropObserverOnTriggerEnter(Collider other) =>
            TryCollect(other);

        private void DropObserverOnTriggerStay(Collider other) =>
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