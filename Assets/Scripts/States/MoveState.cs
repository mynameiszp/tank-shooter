using UnityEngine;
using Zenject;

public class MoveState : IState
{
    [Inject] private readonly ITankAI _tankAI;
    private Vector3 _movementDirection;
    private Transform _enemy;
    private float _moveSpeed;

    public void Initialize(Transform enemy, float moveSpeed)
    {
        _enemy = enemy;
        _moveSpeed = moveSpeed;
    }

    public void Enter()
    {
        _movementDirection = _tankAI.GetMovementDirection();
    }
    public void Update()
    {
        _enemy.transform.Translate(_movementDirection * _moveSpeed);
    }

    public void Exit()
    {
        Debug.Log("Exit moving state");
    }
}
