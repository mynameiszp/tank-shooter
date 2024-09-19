using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationDegree;
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
        transform.Translate(new Vector2(_moveInput.y * _movementSpeed, 0));
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, -_moveInput.x * _rotationDegree);
    }
}
