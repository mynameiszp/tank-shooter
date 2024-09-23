using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private IMovementStrategy _moveStrategy;

    private Vector2 _moveInput;
    private PlayerTank _playerTank;

    private void Awake()
    {
        if (TryGetComponent(out PlayerTank playerTank))
        {
            _playerTank = playerTank;
        }
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputValue input)
    {
        _moveInput = input.Get<Vector2>();
    }

    public void OnFire(InputValue input)
    {
        if (_playerTank == null) return;
        _playerTank.Fire();
    }

    public void ResetInput()
    {
        _moveInput = Vector2.zero;
    }

    private void Move()
    {
        transform.Translate(_moveStrategy.GetDirection(_moveInput));
    }

    private void Rotate()
    {
        transform.Rotate(_moveStrategy.GetRotation(_moveInput));
    }
}
