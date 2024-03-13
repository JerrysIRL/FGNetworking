using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    private Vector2 _cursorLocation;

    private Rigidbody2D _rb;
    private Transform _turretPivotTransform;

    public UnityAction<bool> OnFireEvent;
    public UnityAction MissileLaunchEvent;

    public float boostCooldown = 2f;
    private readonly NetworkVariable<bool> _isBoostAvailable = new NetworkVariable<bool>(true);
    
    
    [Header("Sprite Renderer")] [SerializeField]
    Sprite movingSprite, stationarySprite;

    private SpriteRenderer _renderer;
    private NetworkVariable<bool> _isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Settings")] 
    [SerializeField] private float shipRotationSpeed = 100f;
    [SerializeField] private float turretRotationSpeed = 4f;
    private NetworkVariable<float> _movementSpeed = new NetworkVariable<float>(5f);

    public override void OnNetworkSpawn()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _isMoving.OnValueChanged += UpdateSprite;
       
        if (!IsOwner)
            return;
        
        if (_playerInput == null)
        {
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }

        _playerInput.Player.Enable();
        _rb = GetComponent<Rigidbody2D>();
        _turretPivotTransform = transform.Find("PivotTurret");

        if (_turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnFireEvent.Invoke(true);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _rb.velocity = transform.up * (_moveInput.y * _movementSpeed.Value);
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
        _isMoving.Value = _rb.velocity.magnitude > 0;
    }


    private void LateUpdate()
    {
        if (!IsOwner)
            return;
        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - _turretPivotTransform.position.x, screenToWorldPosition.y - _turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(_turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        _turretPivotTransform.up = currentDirection;
    }

    private void UpdateSprite(bool previousvalue, bool newValue)
    {
        _renderer.sprite = _isMoving.Value ? movingSprite : stationarySprite;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _cursorLocation = context.ReadValue<Vector2>();
    }

    public void OnFireHomingMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
            MissileLaunchEvent.Invoke();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isBoostAvailable.Value)
                BoostRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void BoostRpc()
    {
        StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        _isBoostAvailable.Value = false;
        _movementSpeed.Value *= 2;
        yield return new WaitForSeconds(0.5f);
        _movementSpeed.Value /= 2;
        yield return new WaitForSeconds(boostCooldown);
        _isBoostAvailable.Value = true;
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
            _playerInput.Dispose();

        _isMoving.OnValueChanged -= UpdateSprite;
    }
}