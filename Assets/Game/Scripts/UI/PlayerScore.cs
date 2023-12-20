using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO playerCards;
    private TextMeshProUGUI playerScore;

    private void Start()
    {
        playerScore = GetComponent<TextMeshProUGUI>();
        playerScore.text = null;
    }

    private void Update()
    {
        playerScore.text = "Score:" + playerCards.data.Count;
    }
}
