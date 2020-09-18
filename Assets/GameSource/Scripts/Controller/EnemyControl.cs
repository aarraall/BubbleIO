using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyControl : MonoBehaviour
{
    public static EnemyControl Instance;
    public String[] names;
    public Rigidbody myRb;
    public Animator animator;
    public Player player;
    private Vector3 _currentDest;
    public NavMeshAgent navMeshAgent;
    public Transform[] destPoints;
    public ParticleSystem attackParticle;
    public float closestDistance = Mathf.Infinity;
    public float chaseRange;
    public Color color;
    private float _reachTime = 30;
    public ParticleSystem smokeParticle;

    public SkinnedMeshRenderer[] skinnedMeshRenderers;

    // START
    private void Awake()
    {
        Instance = GetComponent<EnemyControl>();
    }

    void Start()
    {
        _currentDest = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        myRb = GetComponent<Rigidbody>();
        player = new Player(transform, myRb, 5);
        player.color = color;
        player.scale = transform.localScale.x;


        foreach (SkinnedMeshRenderer skin in skinnedMeshRenderers)
        {
            skin.material.color = color;
        }

        player.name = RandomName();
        player.DieMethodEvent += Die;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponentInParent<PlayerControl>() || other.GetComponentInParent<EnemyControl>())
        {
            Die();
        }
    }
    private void Update()
    {
        if (GameManager.isGameOn)
        {
            _reachTime -= Time.deltaTime;
            if ((Vector3.Distance(transform.position, _currentDest) < 1f) ||
                _reachTime < 1)
            {
                animator.SetTrigger("isRunning");
                SetDestination();
                _reachTime = 15;
                
            }
            
        }
    }
    // EAT OTHER PLAYER
    void Die()
    {
        var meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(var mesh in meshes)
        {
            mesh.enabled = false;
        }
        Instantiate(smokeParticle, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        Destroy(gameObject, 1f);
        player.DieMethodEvent -= Die;
    }

    // NEW PATH CHANGE
    public void SetDestination()
    {
        navMeshAgent.SetDestination(SetNewDestinationPoint());
        
    }

    Vector3 SetNewDestinationPoint()
    {
        Vector3 tempDes = destPoints[UnityEngine.Random.Range(0, destPoints.Length)].position;
        while (Vector3.Distance(tempDes, _currentDest) < 0.1f)
        {
            tempDes = destPoints[UnityEngine.Random.Range(0, destPoints.Length)].position;
        }

        _currentDest = tempDes;
        return _currentDest;
    }

    string RandomName()
    {
        return names[UnityEngine.Random.Range(0, names.Length)];
    }
    
}