using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeToCount = 120f;
    public Text timeText;
    

    void Update()
    {
        timeToCount -= Time.deltaTime;
        if (timeToCount >= 0 && GameManager.instance.timeStarted)
        {
            float minutes = Mathf.FloorToInt(timeToCount / 60);
            float seconds = Mathf.FloorToInt(timeToCount % 60);
            timeText.text = "TIME : " + string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        else
        {

            

            if (FindObjectOfType<ScoreManager>().cards[0].player.name == "YOU")
            {
                // RANKS 1ST
                GameManager.instance.FinishLevel();
                Debug.Log("Win - Timer");
            }
            else
            {
                GameManager.instance.GameOver();
                Debug.Log("Game Over - Timer");

            }
        }

    }
}
