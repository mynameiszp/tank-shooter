using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationDegree;
    private Vector2 _moveInput;

    void FixedUpdate()
    {
        MoveHorizontally();
        Rotate();
    }

    public void OnMove(InputValue input)
    {
        _moveInput = input.Get<Vector2>();
    }

    public void OnFire(InputValue input)
    {
        Debug.Log("Fire");
    }

    private void MoveHorizontally()
    {
        transform.Translate(new Vector2(_moveInput.y * _movementSpeed, 0));
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, -_moveInput.x * _rotationDegree);
    }
}
