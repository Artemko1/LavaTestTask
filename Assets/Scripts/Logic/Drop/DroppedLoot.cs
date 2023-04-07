using System.Collections;
using Data;
using Data.DataLoot;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Drop
{
    public class DroppedLoot : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;

        [SerializeField] private float _delayBeforeCollection = 1f;

        [SerializeField] private ConstantForce _constantForce;

        [SerializeField] private Rigidbody _rigidbody;


        private bool _becomeCollectableOverTime;

        private bool _isCollected;

        private Loot _loot;

        public bool CanBeCollected { get; private set; }

        private IEnumerator Start()
        {
            Assert.IsNotNull(_loot);

            if (!_becomeCollectableOverTime) yield break;

            yield return new WaitForSeconds(_delayBeforeCollection);
            CanBeCollected = true;
        }

        public void Init(Loot loot, bool isCollectableDrop)
        {
            Assert.IsNull(_loot);
            _loot = loot;

            if (isCollectableDrop)
            {
                _becomeCollectableOverTime = true;
                _constantForce.enabled = true;
            }
            else
            {
                _becomeCollectableOverTime = false;
                _constantForce.enabled = false;
            }
        }

        public void AddRigidbodyForce(Vector3 force) =>
            _rigidbody.AddForce(force, ForceMode.VelocityChange);

        public void Collect(Vector3 collectorPosition)
        {
            Assert.IsTrue(CanBeCollected, "Should not call Collect() if Drop cannot be collected");
            if (_isCollected) return;

            _isCollected = true;

            DOTween.Sequence()
                .Append(transform.DOJump(collectorPosition, 0.5f, 1, 0.35f))
                .Join(transform.DOScale(0.1f, 0.35f))
                .OnComplete(OnCollectComplete)
                .SetEase(Ease.Linear);
        }

        private void OnCollectComplete()
        {
            _playerProgressProvider.PlayerProgress.LootData.Collect(_loot);
            Destroy(gameObject);
        }
    }
}