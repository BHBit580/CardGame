using System;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private GameObject enemyBot;
    
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
            textTitle.text = "You Lose";
        }
        else if (enemyCards.data.Count == 0)
        {
            enemyBot.SetActive(false);
            textTitle.text = "You Win";
        }
    }
}
