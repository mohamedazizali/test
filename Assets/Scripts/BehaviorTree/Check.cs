using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Check : Node
{
    private Transform _transform;
    private static int _enemyLayerMask = 1 << 6;
    public Check(Transform transform)
    {
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if(t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position,
                GuardBT.fovRange, _enemyLayerMask);
            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
