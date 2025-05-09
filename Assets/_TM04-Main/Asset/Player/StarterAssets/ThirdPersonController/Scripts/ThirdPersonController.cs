﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class ThirdPersonController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _targetRotation2 = 0.0f;

    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private float _angle;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private CinemachineCameraController _cameraController;
    private Transform _startPosition;
    private MobileDisableAutoSwitchControls _mobileDisableAutoSwitchControls;
    private UICanvasControllerInput _uiCanvasControllerInput;

    private Vector2 _inputMove;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private Animator _animator;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;
    private float _weaponState = 0f;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
            return false;
#endif
        }
    }

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        //_animator.SetFloat("State", _weaponState);
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
        Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

        AssignAnimationIDs();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);
        GroundedCheck();
        GravityA();
    }

    private void FixedUpdate()
    {
        Move();
        AnimationMove();
    }

    public void SwapWeapon()
    {
        if (_weaponState == 0)
        {
            _weaponState = 1; 
        }
        else if (_weaponState == 1)
        {
            _weaponState = 0;
        }
        _animator.SetFloat("State", _weaponState);
    }

    public float AngleCalculation(Vector2 vector1, Vector2 vector2)
    {
        float angle = Vector2.SignedAngle(vector1, vector2);
        return angle;
    }

    private float _velocityZ = 0.0f;
    private float _velocityX = 0.0f;
    private float _acceleration = 5.0f;
    private float _currentMaxVelocity = 0.5f;

    private int x;
    private int y;

    private void AnimationMove()
    {
        _input.SetLookTopDown(_input.lookTopDown.normalized);
        _input.SetMove(_input.move.normalized);
        _angle = AngleCalculation(_input.lookTopDown.normalized, _input.move.normalized);
        if (_angle > -45 && _angle < 45)
        {
            y = 1;
            x = 0;
        }
        if ((_angle < 180 && _angle > 135) || (_angle < -135 && _angle > -180))
        {
            y = -1;
            x = 0;
        }
        if (_angle > 45 && _angle < 135)
        {
            x = 1;
            y = 0;
        }
        if (_angle < -45 && _angle > -135)
        {
            x = -1;
            y = 0;
        }
        if (y == 1 && _velocityZ < _currentMaxVelocity)
        {
            _velocityZ += Time.deltaTime * _acceleration;
        }
        if (y == -1 && _velocityZ > -_currentMaxVelocity)
        {
            _velocityZ -= Time.deltaTime * _acceleration;
        }
        if (x == 1 && _velocityX > -_currentMaxVelocity)
        {
            _velocityX -= Time.deltaTime * _acceleration;
        }
        if (x == -1 && _velocityX < _currentMaxVelocity)
        {
            _velocityX += Time.deltaTime * _acceleration;
        }
        if (!(y == 1) && _velocityZ > 0.0f)
        {
            _velocityZ -= Time.deltaTime * _acceleration;
        }
        if (!(y == -1) && _velocityZ < 0.0f)
        {
            _velocityZ += Time.deltaTime * _acceleration;
        }
        if (!(x == 1) && _velocityX < 0.0f)
        {
            _velocityX += Time.deltaTime * _acceleration;
        }
        if (!(x == -1) && _velocityX > 0.0f)
        {
            _velocityX -= Time.deltaTime * _acceleration;
        }
        _animator.SetFloat("VelocityX", _velocityX);
        _animator.SetFloat("VelocityZ", _velocityZ);
    }

    public void Aim()
    {
        Collider nearestCollider = FindClosestCollider(transform.position, 30f);
        transform.LookAt(nearestCollider.transform.position);
    }

    Collider FindClosestCollider(Vector3 center, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Enemy"));

        if (colliders.Length == 0) return null;

        Collider closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            float distance = Vector3.Distance(center, col.transform.position);

            if (col.gameObject == gameObject) continue;

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col;
            }
        }

        return closest;
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        float targetSpeed = (_input.lookTopDown == Vector2.zero || _weaponState == 0) ? SprintSpeed : MoveSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
          currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if (targetSpeed >= SprintSpeed) _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed + 6, Time.deltaTime * SpeedChangeRate);
        else _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        Vector3 inputDirection2 = new Vector3(_input.lookTopDown.x, 0.0f, _input.lookTopDown.y).normalized;

        if (_input.move != Vector2.zero || _input.lookTopDown != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            _targetRotation2 = Mathf.Atan2(inputDirection2.x, inputDirection2.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;

            if (_input.lookTopDown != Vector2.zero && _weaponState != 0)
            {
                float rotation2 = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation2, ref _rotationVelocity,
                    RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation2, 0.0f);
            }
            else
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        if (_speed != 0)
        {
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    // private void JumpAndGravity()
    // {
    //     if (Grounded)
    //     {
    //         _fallTimeoutDelta = FallTimeout;
    //
    //         if (_hasAnimator)
    //         {
    //             _animator.SetBool(_animIDJump, false);
    //             _animator.SetBool(_animIDFreeFall, false);
    //         }
    //
    //         if (_verticalVelocity < 0.0f)
    //         {
    //             _verticalVelocity = -2f;
    //         }
    //
    //         if (_input.jump && _jumpTimeoutDelta <= 0.0f)
    //         {
    //             _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
    //
    //             if (_hasAnimator)
    //             {
    //                 _animator.SetBool(_animIDJump, true);
    //             }
    //         }
    //
    //         if (_jumpTimeoutDelta >= 0.0f)
    //         {
    //             _jumpTimeoutDelta -= Time.deltaTime;
    //         }
    //     }
    //     else
    //     {
    //         _jumpTimeoutDelta = JumpTimeout;
    //
    //         if (_fallTimeoutDelta >= 0.0f)
    //         {
    //             _fallTimeoutDelta -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             if (_hasAnimator)
    //             {
    //                 _animator.SetBool(_animIDFreeFall, true);
    //             }
    //         }
    //
    //         _input.jump = false;
    //     }
    //
    //     if (_verticalVelocity < _terminalVelocity)
    //     {
    //         _verticalVelocity += Gravity * Time.deltaTime;
    //     }
    // }
    
    private void GravityA()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDFreeFall, false);
            }

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
        }
        else
        {
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            float health = PlayerManager.Instance.GetHealth();
            health -= 20;
            PlayerManager.Instance.SetHealth(health);
        }

        if (other.gameObject.CompareTag("Crate"))
        {
            // Debug.Log(other.gameObject.name);
            InventoryManager.Instance.currentTypeShow = InventoryManager.TypeShow.Crate;
            InventoryManager.Instance.currentCrate = other.gameObject.GetComponentInParent<Crate>();
            InventoryManager.Instance.currentItemCrate = other.gameObject.GetComponentInParent<Crate>().items;
            Item[] items = other.gameObject.GetComponentInParent<Crate>().items;
            InventoryManager.Instance.AddItemCrate(items);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Crate"))
        {
            InventoryManager.Instance.currentTypeShow = InventoryManager.TypeShow.Inventory;
            InventoryManager.Instance.DestroyAllItemInCrate();
        }
    }
}