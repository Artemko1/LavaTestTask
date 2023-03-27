using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Player
{
    public class DepositMiner : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _depositObserver;

        [SerializeField] private float _miningCooldown = 2f;

        private readonly List<Deposit> _nearDeposits = new List<Deposit>();
        private float _remainingCooldown;

        private void Update() => 
            _remainingCooldown -= Time.deltaTime;

        private void OnEnable()
        {
            _depositObserver.TriggerEnter += DepositTriggerEnter;
            _depositObserver.TriggerExit += DepositTriggerExit;
        }

        private void OnDisable()
        {
            _depositObserver.TriggerEnter -= DepositTriggerEnter;
            _depositObserver.TriggerExit -= DepositTriggerExit;

            _nearDeposits.Clear();
        }

        private void DepositTriggerEnter(Collider other)
        {
            var deposit = other.GetComponent<Deposit>();
            Assert.IsNotNull(deposit);
            _nearDeposits.Add(deposit);
        }

        private void DepositTriggerExit(Collider other)
        {
            var deposit = other.GetComponent<Deposit>();
            _nearDeposits.Remove(deposit);
        }

        public void TryMine()
        {
            if (!ReadyToMine()) return;

            Deposit nearestDeposit = FindNearestDeposit();
            nearestDeposit.Mine();
            _remainingCooldown = _miningCooldown;
        }

        private bool ReadyToMine() =>
            _remainingCooldown < 0 && _nearDeposits.Count > 0;

        private Deposit FindNearestDeposit()
        {
            Deposit bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Deposit deposit in _nearDeposits)
            {
                Vector3 directionToTarget = deposit.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget >= closestDistanceSqr) continue;

                closestDistanceSqr = dSqrToTarget;
                bestTarget = deposit;
            }

            return bestTarget;
        }
    }
}