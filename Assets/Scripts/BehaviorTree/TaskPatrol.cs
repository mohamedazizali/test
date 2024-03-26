using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskPatrol : Node
{
    private Transform _transform;
    private Transform[] _waypoints;
    private Animator _animator;
    private int _currentWayPointIndex = 0;
    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        _animator = transform.GetComponent<Animator>();

    }
    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
                _waiting = false;
            _animator.SetBool("Walking", true);
        }
        else
        {
            Transform wp = _waypoints[_currentWayPointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;
                _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Length;
                _animator.SetBool("Walking", false);
            }
            else {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                _transform.LookAt(wp.position);
                Vector3 euler = _transform.rotation.eulerAngles;
                euler.x = 0f; // Set X axis rotation to 0
                _transform.rotation = Quaternion.Euler(euler);
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
}
