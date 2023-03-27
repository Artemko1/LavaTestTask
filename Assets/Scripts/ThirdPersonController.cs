using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("How fast the character turns to face movement direction"), Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;

        // animation IDs
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        private Animator _animator;
        private CharacterController _controller;
        private GameObject _mainCamera;
        private PlayerInput _playerInput;

        private void Start()
        {
            _mainCamera = Camera.main.gameObject;
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update() =>
            Move();


        private void Move()
        {
            Vector2 moveVector = _playerInput.Axis;

            float targetSpeed = MoveSpeed;

            if (moveVector == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float inputMagnitude = moveVector.magnitude;
            
            float speed = CalculateSpeed(currentHorizontalSpeed, targetSpeed, inputMagnitude);

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(moveVector.x, 0.0f, moveVector.y).normalized;

            // if there is a move input rotate player when the player is moving
            if (moveVector != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _controller.Move(targetDirection.normalized * (speed * Time.deltaTime));

            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        private float CalculateSpeed(float currentHorizontalSpeed, float targetSpeed, float inputMagnitude)
        {
            const float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                return Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
            }

            return targetSpeed;
        }
    }
}