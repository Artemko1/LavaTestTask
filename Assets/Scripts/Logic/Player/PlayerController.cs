using Logic.Deposits;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Player
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(PlayerAnimator)), RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private DepositMiner _depositMiner;
        private NavMeshAgent _agent;
        private PlayerAnimator _playerAnimator;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            ProcessMove();

            if (IsNotMoving())
            {
                _depositMiner.TryMine();
            }
        }


        private bool IsNotMoving() =>
            !_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance &&
            _agent.pathStatus == NavMeshPathStatus.PathComplete;

        private void ProcessMove()
        {
            Vector3 moveDirection = new Vector3(_playerInput.Horizontal, 0f, _playerInput.Vertical).normalized;

            if (moveDirection != Vector3.zero)
            {
                _agent.SetDestination(transform.position + moveDirection);
            }
            else
            {
                _agent.ResetPath();
            }

            _playerAnimator.PlayMove(_agent.velocity.magnitude);
        }
    }
}