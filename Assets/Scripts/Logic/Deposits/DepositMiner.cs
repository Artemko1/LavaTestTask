using System.Collections.Generic;
using Logic.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Deposits
{
    public class DepositMiner : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private DepositMinerSettings _settings;

        private readonly List<Deposit> _nearDeposits = new List<Deposit>();
        private float _remainingCooldown;

        private void Update()
        {
            _remainingCooldown -= Time.deltaTime;

            if (ReadyToMine())
            {
                TryMine();
            }
        }

        private void OnEnable()
        {
            var capsule = GetComponent<CapsuleCollider>();
            capsule.radius = _settings.MiningRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            var deposit = other.GetComponent<Deposit>();
            Assert.IsNotNull(deposit);
            _nearDeposits.Add(deposit);
        }

        private void OnTriggerExit(Collider other)
        {
            var deposit = other.GetComponent<Deposit>();
            _nearDeposits.Remove(deposit);
        }

        private void TryMine()
        {
            Deposit nearestDeposit = FindNearestAvailableDeposit();
            if (nearestDeposit == null) return;

            nearestDeposit.Mine();
            _remainingCooldown = nearestDeposit.MiningCooldown;
        }

        private bool ReadyToMine() =>
            _characterMovement.IsMoving == false && _remainingCooldown < 0 && _nearDeposits.Count > 0;

        private Deposit FindNearestAvailableDeposit()
        {
            Deposit bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Deposit deposit in _nearDeposits)
            {
                if (!deposit.CanBeMined()) continue;

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