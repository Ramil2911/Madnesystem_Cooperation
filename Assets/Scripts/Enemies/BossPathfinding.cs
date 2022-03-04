using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathfinding : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 2;
    private UnityEngine.AI.NavMeshAgent _agent;
    public float distanceReaction = 10;
    public bool canAttack = false;
    public bool playerIsNear = false;
    public bool isAttacker = true;

    private void Start()
    {
        
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        Pathfinding();
    }

    private void Pathfinding()
    {
        AttackPlayer();
    }

    private void CheckVisibility()
    {
        if (Vector3.Distance(transform.position, _player.position) <= distanceReaction)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    private void AttackPlayer() {
        CheckVisibility();
        if (canAttack) {
            _agent.speed = 0.01f;
            _agent.angularSpeed = 360f;
        }
        else {
            FollowPoint(_player);
        }
    }

    private void FollowPoint(Transform point) {
        _agent.SetDestination(point.position);
        _agent.speed = _speed;
        _agent.angularSpeed = 120f;
    }


}
