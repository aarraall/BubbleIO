using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;


[Serializable]
public class Player
{
    public Player(Transform transform, Rigidbody rigidbody, float speed)
    {
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.speed = speed;
    }

    public delegate void DieMethod();

    public Transform transform;
    public Rigidbody rigidbody;
    public string name;
    public float speed;
    public event DieMethod DieMethodEvent;
    public bool isLive = true;
    public float scale;
    public Color color;


    // EAT COOKIE
    public void GetBigger(Transform enemy, ParticleSystem attackParticle)
    {
        if (enemy.localScale.x < transform.localScale.x || enemy.CompareTag("Cookie"))
        {
            BiggerMecha(transform, enemy, attackParticle);
        }
        else
        {
            isLive = false;
            if (transform.CompareTag("Player") || transform.CompareTag("Enemy"))
            {
                GameManager.instance.diedPlayerCount++;
                
                float temp = (GameManager.instance.diedPlayerCount / 10f) + 0.1f;
                transform.localScale = new Vector3(temp, temp, temp);
                scale = temp;
                name = "Killed";
                

                // ENEMY KILL TO ALL ENEMY AND I
                if (GameManager.instance.diedPlayerCount == 4 && PlayerControl.Instance.name == "Player")
                {
                    GameManager.instance.FinishLevel();

                }

            }

            DieMethodEvent?.Invoke();
        }
        
        ScoreManager.instance.OrderList();
    }

    private void BiggerMecha(Transform destBigger, Transform eatenScale, ParticleSystem attackParticle)
    {
        Vector3 destSize = destBigger.localScale + (eatenScale.transform.localScale / 10);
        destBigger.DOScale(destSize, 0.3f);
        scale = destSize.x;
        //Todo make bigger particle meshes
        var particleSize = attackParticle.transform;
        var particleIncrement = new Vector3(0.05f, 0.05f, 0.05f);
        particleSize.DOScale(particleIncrement, .1f).SetRelative();
        particleSize.DORotate(new Vector3(1.25f, 0, 0), .1f).SetRelative();
    }
   
}