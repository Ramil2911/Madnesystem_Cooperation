using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 2;
    private NavMeshAgent _agent;
    public float distanceReaction = 10;
    public bool canAttack = false;
    public bool playerIsNear = false;
    public bool isAttacker = false;
    public Transform holdPosition = null;

    private void Start()
    {
        
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Pathfinding();
    }

    private void Pathfinding()
    {
        if (isAttacker) {
            AttackPlayer();
            return;
        } 
        FollowHoldPosition();
    }

    private void CheckVisibility()
    {
        if (Physics.Linecast(transform.position, _player.position + _player.transform.up) && Vector3.Distance(transform.position, _player.position) <= distanceReaction)
        {
            Debug.DrawLine(transform.position, _player.position);
            _agent.speed = 0.01f;
            _agent.angularSpeed = 360f;
            canAttack = true;
        }
        else
        {
            Debug.DrawLine(transform.position, _player.position);
            canAttack = false;
        }
    }

    private void AttackPlayer() {
        if (playerIsNear == false)
        {
            FollowPoint(_player);
            canAttack = false;
        }
        else
        {
            CheckVisibility();
        }
    }

    private void FollowHoldPosition() {
        FollowPoint(holdPosition);
    }

    private void FollowPoint(Transform point) {
        _agent.SetDestination(point.position);
        _agent.speed = _speed;
        _agent.angularSpeed = 120f;
    }


}