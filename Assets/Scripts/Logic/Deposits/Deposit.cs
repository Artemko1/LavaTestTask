using UnityEngine;

namespace Logic.Deposits
{
    public class Deposit : MonoBehaviour
    {
        [SerializeField] private DepositView _depositView;

        [SerializeField] private int _remainingResource = 3;

        [SerializeField] private float _miningCooldown = 2f;

        [SerializeField] private int _resourceDropPerMine = 1;


        public float MiningCooldown => _miningCooldown;

        private bool IsMinedOut => _remainingResource <= 0;

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

            _depositView.PlayMining(_remainingResource);
            _depositView.DropResources(_resourceDropPerMine);
        }
    }
}