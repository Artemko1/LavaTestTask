using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Deposits
{
    [SelectionBase]
    public class Deposit : MonoBehaviour
    {
        [SerializeField] private DepositView _depositView;
        [SerializeField] private DepositSettings _settings;

        private int _remainingMining;

        private float _remainingRestorationTime;


        public float MiningCooldown => _settings.MiningCooldown;
        public LootType LootType => _settings.LootDropPerMine.Type;

        private bool IsMinedOut => _remainingMining <= 0;

        private void Awake()
        {
            ResetRestorationTimer();
            _remainingMining = _settings.MaxMining;

            Assert.IsNotNull(_settings);
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

            _remainingMining--;
            _depositView.ChangeView(_remainingMining);

            _depositView.PlayMiningAnimation();
            _depositView.DropLoot(_settings.LootDropPerMine);

            ResetRestorationTimer();
        }

        private void ResetRestorationTimer() =>
            _remainingRestorationTime = _settings.RestorationTime;

        private bool IsFull() =>
            _remainingMining >= _settings.MaxMining;

        private void TickRestoration()
        {
            _remainingRestorationTime -= Time.deltaTime;
            if (_remainingRestorationTime > 0) return;

            Assert.AreNotEqual(_remainingMining, _settings.MaxMining);

            _remainingMining++;
            _depositView.ChangeView(_remainingMining);
            ResetRestorationTimer();
        }
    }
}