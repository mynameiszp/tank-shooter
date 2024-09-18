using UnityEngine;
using Zenject;

public class RotateState : IState
{
    [Inject] private readonly ITankAI _tankAI;

    private float _rotationSpeed;
    private float _rotationDegree;
    private float _rotationInterval;
    private Quaternion _targetRotation;
    private Transform _enemy;

    public void Initialize(Transform enemy, float rotationInterval, float rotationDegree)
    {
        _enemy = enemy;
        _rotationDegree = rotationDegree;
        _rotationInterval = rotationInterval;
        _rotationSpeed = _rotationDegree / _rotationInterval;
    }

    public void Enter()
    {
        _targetRotation = _enemy.transform.rotation * _tankAI.GetRotation(new Vector3(0, 0, _rotationDegree));
    }

    public void Update()
    {
        _enemy.transform.rotation = Quaternion.RotateTowards(
                _enemy.transform.rotation,
                _targetRotation,
                _rotationSpeed * Time.fixedDeltaTime
            );
    }

    public void Exit()
    {
        Debug.Log("Exit rotation state");
    }
}
