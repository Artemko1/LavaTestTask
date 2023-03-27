using UnityEngine;

namespace Logic.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");

        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void PlayMove(float speed) =>
            _animator.SetFloat(_animIDSpeed, speed);
    }
}