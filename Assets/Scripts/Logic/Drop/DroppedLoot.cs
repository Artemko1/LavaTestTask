using System.Collections;
using Data;
using Data.DataLoot;
using DG.Tweening;
using UnityEngine;

namespace Logic.Drop
{
    public class DroppedLoot : MonoBehaviour
    {
        [SerializeField]
        private PlayerProgressProvider _playerProgressProvider;
        
        [SerializeField] private float _delayBeforeCollection = 1f;
        [SerializeField] private Loot _loot;
        
        private bool _collected;

        public bool CanBeCollected { get; private set; }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delayBeforeCollection);
            CanBeCollected = true;
        }

        public void Collect(Vector3 collectorPosition)
        {
            if (_collected || !CanBeCollected)
            {
                return;
            }

            _collected = true;

            DOTween.Sequence()
                .Append(transform.DOJump(collectorPosition, 0.5f, 1, 0.35f))
                .Join(transform.DOScale(0.1f, 0.35f))
                .OnComplete(OnSequenceComplete)
                .SetEase(Ease.Linear);
            
        }

        private void OnSequenceComplete()
        {
            _playerProgressProvider.PlayerProgress.LootData.Collect(_loot);
            Destroy(gameObject);
        }
    }
}