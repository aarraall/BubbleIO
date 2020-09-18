using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl Instance;
    

    [HideInInspector] public Rigidbody myRb;

    public Joystick joystick;
    public Animator playerAnim;    
    public GameObject plusText, killText, youKill;

    public Player player;    
    public ParticleSystem attackParticle;
    private Vector3 lastVector3;
   
    public int killCount; 
    public Color color;
    private int startCount;
    private int playerKillCount;
    public SkinnedMeshRenderer[] skinnedMeshRenderer;



    // START SET
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        myRb = GetComponent<Rigidbody>();
        player = new Player(transform, myRb, 7);
        player.color = color;
        player.scale = transform.localScale.x;
        attackParticle.gameObject.SetActive(false);


        foreach (SkinnedMeshRenderer skin in skinnedMeshRenderer)
        {
            skin.material.color = color;
        }

        player.name = "YOU";
        player.DieMethodEvent += Die;
    }

    // JOYTSICK CONTROLLER
    private void Update()
    {
        TurnSystem();
    }

    private void TurnSystem()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        { 
            if(startCount<1)
                startCount++;
            Shoot(false);
            lastVector3 = transform.forward;
            transform.forward = new Vector3(-joystick.Vertical, 0, joystick.Horizontal);
            myRb.velocity = transform.forward * player.speed;
        }
        else
        {
            if (startCount >= 1)
            {
                Shoot(true);
            }
            transform.forward = lastVector3;
            myRb.velocity = Vector3.zero;
        }
    }

    private void Shoot(bool isActive)
    {
        playerAnim.SetBool("isRunning", !isActive);
        playerAnim.SetBool("isFiring", isActive);
        attackParticle.gameObject.SetActive(isActive);
        ParticleSystem.EmissionModule emissionModule = attackParticle.emission;
        emissionModule.enabled = isActive;
    }

    // EAT COOKIE AND KILL OTHER ENEMY
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponentInParent<EnemyControl>())
        {
            player.GetBigger(transform, attackParticle);
            playerKillCount++;
            Die();


            // EAT ENEMY
            if (other.GetComponentInParent<EnemyControl>() && gameObject.transform.localScale.x > other.gameObject.transform.parent.localScale.x)
            {
                killCount++;
                Instantiate(killText, transform.position + new Vector3(0f, 5f, 0f), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                GameManager.instance.kill.text = "ELIMINATED : " + killCount.ToString();

                // I KILL ALL ENEMY
                if (killCount == 4 && player.isLive == true)
                {
                    GameManager.instance.FinishLevel();
                    
                }
            }

        }
    }
    // PLAYER DIE -- GAME OVER

    private void Die()
    {
        var meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(var mesh in meshes)
        {
            mesh.enabled = false;
        }
        player.DieMethodEvent -= Die;
        GameManager.instance.GameOver();
        Debug.Log("Game Over - DIE");
       
    }
}