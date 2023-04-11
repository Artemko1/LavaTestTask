using System;
using Data;
using Data.DataLoot;
using Logic.Deposits;
using Logic.Spots;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private Transform _arrowImageParent;
        [SerializeField] private Transform _target;

        private Settings _settings;

        private void Start()
        {
            Assert.AreEqual(_settings.DepositToFarm.LootType, _settings.SpotToGoTo.RemainingRequiredLoot.Type,
                "Loot types of deposit and spot must be equal. Tutorial setup incorrectly");

            _playerProgressProvider.PlayerProgress.LootData.Collected += OnLootCollected;
            _settings.SpotToGoTo.RefineStart += OnSpotRefineStart;

            Deposit deposit = _settings.DepositToFarm;

            SetTrackTarget(deposit.transform);
        }

        private void Update()
        {
            if (_target == null) return;

            ArrowPointToTarget();
        }

        private void OnDestroy()
        {
            _playerProgressProvider.PlayerProgress.LootData.Collected -= OnLootCollected;
            _settings.SpotToGoTo.RefineStart -= OnSpotRefineStart;
        }

        private void OnSpotRefineStart()
        {
            SetTrackTarget(null);
            _settings.SpotToGoTo.RefineStart -= OnSpotRefineStart;
        }

        private void ArrowPointToTarget()
        {
            Vector3 dir = _target.position - transform.position;
            dir.y = 0;
            float angle = Vector3.SignedAngle(dir, Vector3.forward, Vector3.up);
            _arrowImageParent.eulerAngles = new Vector3(90, 0, angle);
        }

        public void Init(Settings settings) =>
            _settings = settings;

        private void OnLootCollected(LootUpdatedArgs args)
        {
            if (!EnoughLootCollected(args)) return;

            SetTrackTarget(_settings.SpotToGoTo.transform);
            _playerProgressProvider.PlayerProgress.LootData.Collected -= OnLootCollected;
        }

        private bool EnoughLootCollected(LootUpdatedArgs args) =>
            args.Type == _settings.DepositToFarm.LootType &&
            args.TotalAmount >= _settings.SpotToGoTo.RemainingRequiredLoot.Amount;

        private void SetTrackTarget(Transform newTarget)
        {
            _arrowImageParent.gameObject.SetActive(newTarget != null);
            _target = newTarget;
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public bool EnableTutorial { get; private set; } = true;
            [field: SerializeField] public Deposit DepositToFarm { get; private set; }
            [field: SerializeField] public Spot SpotToGoTo { get; private set; }
        }
    }
}