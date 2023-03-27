using UnityEngine;

namespace Logic.Player
{
    public class Deposit : MonoBehaviour
    {
        [SerializeField]
        private int _remainingResource = 3;

        public void Mine()
        {
            _remainingResource--;

            Debug.Log($"Mined! Remaining {_remainingResource}", this);
        }
    }
}