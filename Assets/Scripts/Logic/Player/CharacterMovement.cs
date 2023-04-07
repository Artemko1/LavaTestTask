using UnityEngine;
using UnityEngine.AI;

namespace Logic.Player
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(PlayerAnimator)), RequireComponent(typeof(PlayerInput))]
    public class CharacterMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private PlayerAnimator _playerAnimator;
        private PlayerInput _playerInput;

        public bool IsMoving { get; private set; }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update() =>
            ProcessMove();

        private void ProcessMove()
        {
            Vector3 moveDirection = new Vector3(_playerInput.Horizontal, 0f, _playerInput.Vertical).normalized;

            if (moveDirection != Vector3.zero)
            {
                _agent.SetDestination(transform.position + moveDirection);
                IsMoving = true;
            }
            else
            {
                _agent.ResetPath();
                IsMoving = false;
            }

            _playerAnimator.PlayMove(_agent.velocity.magnitude);
        }
    }
}