using System;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private GameObject enemyBot;
    [SerializeField] private GameObject player;
    
    private TextMeshProUGUI textTitle;

    private void Start()
    {
        textTitle = GetComponent<TextMeshProUGUI>();
        textTitle.text = null;
    }

    private void Update()
    {
        if (playerCards.data.Count == 0)
        {
            enemyBot.SetActive(false);
            player.SetActive(false);
            textTitle.text = "You Lose";
        }
        else if (enemyCards.data.Count == 0)
        {
            enemyBot.SetActive(false);
            player.SetActive(false);
            textTitle.text = "You Win";
        }
    }
}
