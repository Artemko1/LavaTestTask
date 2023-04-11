using System.Collections;
using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Drop
{
    public class DroppedLoot : MonoBehaviour
    {
        [SerializeField] private DroppedLootSettings _droppedLootSettings;
        [SerializeField] private ConstantForce _constantForce;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _becomeCollectableOverTime;
        private bool _canBeCollectedByTime;
        private bool _isCollected;

        public Loot Loot { get; private set; }
        public bool CanBeCollected => _canBeCollectedByTime && !_isCollected;

        private IEnumerator Start()
        {
            Assert.IsNotNull(Loot);

            if (!_becomeCollectableOverTime) yield break;

            yield return new WaitForSeconds(_droppedLootSettings.DelayBeforeCollection);
            _canBeCollectedByTime = true;
        }

        public void Init(Loot loot, bool isCollectableDrop)
        {
            Assert.IsNull(Loot);
            Loot = loot;

            if (isCollectableDrop)
            {
                _becomeCollectableOverTime = true;
                _constantForce.enabled = true;
                _constantForce.force.Set(0, _droppedLootSettings.GravityForce, 0);
            }
            else
            {
                _becomeCollectableOverTime = false;
                _constantForce.enabled = false;
            }
        }

        public void AddRigidbodyForce(Vector3 force) =>
            _rigidbody.AddForce(force, ForceMode.VelocityChange);

        public void MarkCollected() =>
            _isCollected = true;
    }
}