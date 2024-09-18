using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    [Inject] private readonly StateMachine _stateMachine;
    [Inject] private readonly RotateState _rotateState;
    [Inject] private readonly MoveState _moveState;

    [SerializeField] private float _movementInterval;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationInterval;
    [SerializeField] private float _rotationDegree;

    private IEnumerator _stateCoroutine;
    private int _boundaryLayer;
    private int _playerLayer;

    private const string PLAYER_LAYER = GameConstants.PLAYER_LAYER_NAME;
    private const string BOUNDARY_LAYER = GameConstants.BOUNDARY_LAYER_NAME;

    void Start()
    {
        InitializeLayers();
        InitializeStates();
        InitializeStateMachine();
    }

    private void InitializeLayers()
    {
        _playerLayer = LayerMask.NameToLayer(PLAYER_LAYER);
        _boundaryLayer = LayerMask.NameToLayer(BOUNDARY_LAYER);
    }

    private void InitializeStates()
    {
        _rotateState.Initialize(transform, _rotationInterval, _rotationDegree);
        _moveState.Initialize(transform, _moveSpeed);
    }

    private void InitializeStateMachine()
    {
        _stateMachine.OnStateChanged += ChangeState;
        _stateMachine.Initialize(_moveState);
    }

    private void ChangeState(IState state)
    {
        float intervalDuration = GetStateInterval(state);
        IState nextState = GetNextState(state);

        if (nextState != null)
        {
            RestartStateTimer(nextState, intervalDuration);
        }
    }
    private float GetStateInterval(IState state)
    {
        if (state == _moveState)
            return _movementInterval;
        else if (state == _rotateState)
            return _rotationInterval;

        return 0f;
    }

    private IState GetNextState(IState currentState)
    {
        if (currentState == _moveState)
            return _rotateState;
        else if (currentState == _rotateState)
            return _moveState;

        return null;
    }
    private void RestartStateTimer(IState nextState, float time)
    {
        if (_stateCoroutine != null)
        {
            StopCoroutine(_stateCoroutine);
        }

        _stateCoroutine = TransitionAfterDelay(nextState, time);
        StartCoroutine(_stateCoroutine);
    }

    private IEnumerator TransitionAfterDelay(IState nextState, float time)
    {
        yield return new WaitForSeconds(time);
        _stateMachine.TransitionTo(nextState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == _playerLayer || collisionLayer == _boundaryLayer)
        {
            StopCoroutine(_stateCoroutine);
            _stateMachine.TransitionTo(_rotateState);
        }
    }
}
