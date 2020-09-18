using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;


    public GameObject player, finishPanel, gameOverPanel, gamePanel, menuPanel;
    public GameObject confeti, cookies;
    public GameObject AIOne, AITwo, AIThree, AIFour;

    public CinemachineVirtualCamera cam;
    public DynamicJoystick joystick;
    public Text time, kill;   
    public Animator playerAnim;

    public int diedPlayerCount;

    public static bool isGameOn;
    public bool timeStarted = false;
    private bool finishGame = false;   
    private bool endGA = true;
    

    // START
    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
      
    }

    // GAME EVENTS

    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }

    public void FinishLevel()
    {
       
        StartCoroutine("FinishPanel");
        playerAnim.Play("Idle");
        confeti.SetActive(true);

        cookies.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        isGameOn = false;
        finishGame = true;
        timeStarted = false;

        StopAI();      

    }

    IEnumerator FinishPanel()
    {
        yield return new WaitForSeconds(5f);
        finishPanel.SetActive(true);
    }

    public void GameOver()
    {

        if (finishGame == false)
        {
            isGameOn = false;
            StartCoroutine("OverPanel");
            playerAnim.Play("Death");

            cookies.SetActive(false);
            player.GetComponent<PlayerControl>().enabled = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        StopAI();
        timeStarted = false;

    }

    IEnumerator OverPanel()
    {
        yield return new WaitForSeconds(3f);
        gameOverPanel.SetActive(true);
    }


    // OTHERS

    public void StartButton()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        cam.m_Priority = 5;

        isGameOn = true;
        timeStarted = true;
        var targets = FindObjectsOfType<Target>();
        foreach(var target in targets)
        {
            target.GetComponent<Target>().enabled = true;   
        }


        EnemyControl[] enemyControls = FindObjectsOfType<EnemyControl>();
        for (int i = 0; i < enemyControls.Length; i++)
        {
            enemyControls[i].SetDestination();
        }
        ScoreManager.instance.FindPlayers();


    }

    public void RestartButton()
    {
        SceneLoad();
    }

    public void StopAI()
    {
        // AI STOP
        AIOne.GetComponent<NavMeshAgent>().enabled = false;
        AITwo.GetComponent<NavMeshAgent>().enabled = false;
        AIThree.GetComponent<NavMeshAgent>().enabled = false;
        AIFour.GetComponent<NavMeshAgent>().enabled = false;
    }

}