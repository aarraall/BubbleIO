using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Serializable]
    public struct ProfileCard
    {
        public Player player;
        public RectTransform rank;
        public Text playerText;
        public ProfileCard(Player player, RectTransform rank,Text playerText)
        {
            this.player = player;
            this.rank = rank;
            this.playerText = playerText;
        }
    }

    public static ScoreManager instance;
    [HideInInspector] public List<Player> players;

    public List<Player> orderedArray;

    public Text[] orderNames;
    public List<RectTransform> rectTransforms;


    public RectTransform[] rankOrder;
    public List<ProfileCard> profileCards;
    public List<ProfileCard> cards;

    // METHODS
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void FindPlayers()
    {
        EnemyControl[] tempPlayers = FindObjectsOfType<EnemyControl>();
        PlayerControl player = FindObjectOfType<PlayerControl>();
        for (int i = 0; i < tempPlayers.Length; i++)
        {
            players.Add(tempPlayers[i].player);
        }

        players.Add(player.player);
        FirstTimeOrderList();
    }

    public void OrderList()
    {
        cards = profileCards.OrderByDescending(x => x.player.transform.localScale.x).ToList();

        
        for (int i = 0; i < rankOrder.Length; i++)
        {
            
            cards[i].rank.DOAnchorPos(rankOrder[i].anchoredPosition, 0.2f);
            if (!cards[i].player.isLive)
            {
                cards[i].playerText.text = "Killed";
            }
        }

       
    }

    private void FirstTimeOrderList()
    {
        orderedArray = players.OrderByDescending(x => x.transform.localScale.x).ToList();
        for (int i = 0; i < orderedArray.Count; i++)
        {
            profileCards.Add(new ProfileCard(orderedArray[i], rectTransforms[i],orderNames[i]));
        }

        for (int i = 0; i < orderNames.Length; i++)
        {
            orderNames[i].text = orderedArray[i].name;
            orderNames[i].color =
                new Color(orderedArray[i].color.r, orderedArray[i].color.g, orderedArray[i].color.b, 1);
        }
    }
}