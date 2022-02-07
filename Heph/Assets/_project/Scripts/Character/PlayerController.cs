using Heph.Scripts.Behaviours.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Heph.Scripts.Character
{
    public class PlayerController : MonoBehaviour
    {
        public Transform cameraTransform;
        public CharacterController motor;

        public Interactor interactionHandler;

        public float speed = 6f;
        public float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;

        private PlayerInputs _playerInputs;
        private InputAction _movementKeys;

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            _movementKeys = _playerInputs.Player.Movement;
            _movementKeys.Enable();

            _playerInputs.Player.Interact.performed += DoInteract;
            _playerInputs.Player.Interact.Enable();
        }

        private void OnDisable()
        {
            _movementKeys.Disable();
            _playerInputs.Player.Interact.Disable();
        }

        private void DoInteract(InputAction.CallbackContext obj)
        {
            interactionHandler.OnFire();
        }

        // Update is called once per frame
        private void Update()
        {
            var movementValue = _movementKeys.ReadValue<Vector2>();
            var direction = new Vector3(movementValue.x, 0f, movementValue.y).normalized;

            // ReSharper disable once InvertIf (Because we are in the update function, so we might want to do something after this
            if(direction.magnitude >= 0.1f)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                motor.Move(moveDirection.normalized * speed * Time.deltaTime);
            }
        }
    }
}
