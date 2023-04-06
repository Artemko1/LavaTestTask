using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Deposits
{
    public class Deposit : MonoBehaviour
    {
        [SerializeField] private DepositView _depositView;

        [SerializeField] private int _maxResource = 3;

        [SerializeField] private Loot _lootDropPerMine;

        [SerializeField] private float _miningCooldown = 2f;

        [SerializeField] private float _restorationTime = 5;

        private int _remainingResource;

        private float _remainingRestorationTime;


        public float MiningCooldown => _miningCooldown;

        private bool IsMinedOut => _remainingResource <= 0;

        private void Awake()
        {
            ResetRestorationTimer();
            _remainingResource = _maxResource;
        }

        private void Update()
        {
            if (IsFull()) return;

            TickRestoration();
        }

        public bool CanBeMined() =>
            !IsMinedOut;

        public void Mine()
        {
            if (IsMinedOut)
            {
                Debug.LogWarning("Already mined out spots should not be mined", this);
                return;
            }

            _remainingResource--;
            _depositView.ChangeView(_remainingResource);

            _depositView.PlayMiningAnimation();
            _depositView.DropLoot(_lootDropPerMine);

            ResetRestorationTimer();
        }

        private void ResetRestorationTimer() =>
            _remainingRestorationTime = _restorationTime;

        private bool IsFull() =>
            _remainingResource >= _maxResource;

        private void TickRestoration()
        {
            _remainingRestorationTime -= Time.deltaTime;
            if (_remainingRestorationTime > 0) return;

            Assert.AreNotEqual(_remainingResource, _maxResource);

            _remainingResource++;
            _depositView.ChangeView(_remainingResource);
            ResetRestorationTimer();
        }
    }
}