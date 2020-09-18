using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Cookies : MonoBehaviour
{
    public Rigidbody myRb;
    public Player player;
    private Vector3 _currentDest;
    public NavMeshAgent navMeshAgent;
    public Transform[] destPoints;
    public ParticleSystem smokeParticle;
    private float _cookieTime;

    void Start()
    {
        _currentDest = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        myRb = GetComponent<Rigidbody>();
        player = new Player(transform, myRb, 2);
        SetDestination();
        navMeshAgent.speed = player.speed;
    }


    private void Update()
    {
        _cookieTime -= Time.deltaTime;

        if (_cookieTime < 1)
        {
            SetDestination();
            _cookieTime = UnityEngine.Random.Range(10, 25);
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponentInParent<PlayerControl>())
        {
            PlayerControl.Instance.player.GetBigger(transform, PlayerControl.Instance.attackParticle);
            var plusText = Instantiate(PlayerControl.Instance.plusText,
                PlayerControl.Instance.transform.position + new Vector3(0f, 3f, 0f),
                Quaternion.Euler(new Vector3(0f, -90f, 0f)));
            plusText.transform.parent = this.transform;
        }
        else if (other.GetComponentInParent<EnemyControl>())
        {
            other.GetComponentInParent<EnemyControl>().player.GetBigger(transform,
                other.GetComponentInParent<EnemyControl>().attackParticle);
            var plusText = Instantiate(PlayerControl.Instance.plusText,
                other.GetComponentInParent<EnemyControl>().transform.position + new Vector3(0f, 3f, 0f),
                Quaternion.Euler(new Vector3(0f, -90f, 0f)));
            plusText.transform.parent = this.transform;
        }

        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        var smokeParticlegO = Instantiate(smokeParticle, transform.position, Quaternion.identity);
        smokeParticlegO.transform.parent = transform;
        SkinnedMeshRenderer[] meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }

        yield return new WaitForSecondsRealtime(.5f);
        Destroy(gameObject);
    }

    void SetDestination()
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
}