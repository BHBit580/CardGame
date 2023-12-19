using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDistribution : MonoBehaviour
{
    public List<GameObject> totalCards = new List<GameObject>(); // Assign 52 card GameObjects in the Inspector
    public ListGameObjectSO playerCards;
    public ListGameObjectSO enemyCards;
    public ListGameObjectSO cardsOnTable;
    
    [SerializeField] private Transform playerLocation , enemyLocation;
    [SerializeField] private float distributionSpeed = 5f;
    [SerializeField] private float timeDelay = 0.5f;

    private void Awake()
    {
        playerCards.data.Clear();
        enemyCards.data.Clear();
        cardsOnTable.data.Clear();
        foreach (Transform child in transform)
        {
            totalCards.Add(child.gameObject);
        }
    }

    void Start()
    {
        CardShuffler();
        StartCoroutine(DistributeCards());
    }

    private void CardShuffler()
    {
        List<GameObject> tempDeck = new List<GameObject>(totalCards);
         
        while (tempDeck.Count > 0)
        {
            int randomIndex = Random.Range(0, tempDeck.Count);
            GameObject selectedCard = tempDeck[randomIndex];
            tempDeck.RemoveAt(randomIndex);

            if (playerCards.data.Count < enemyCards.data.Count)
            {
                playerCards.data.Add(selectedCard);
                selectedCard.layer = LayerMask.NameToLayer("PlayerCard");
            }
            else
            {
                enemyCards.data.Add(selectedCard);
            }
        }
    }
    
    IEnumerator DistributeCards()
    {
        foreach (GameObject card in playerCards.data)
        {
            StartCoroutine(MoveCard(card.transform, playerLocation.position));
            yield return new WaitForSeconds(timeDelay);
        }
        foreach (GameObject card in enemyCards.data)
        {
            StartCoroutine(MoveCard(card.transform, enemyLocation.position));
            yield return new WaitForSeconds(timeDelay);
        }
    }
    
    IEnumerator MoveCard(Transform cardTransform, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = cardTransform.position;

        while (elapsedTime < 1f)
        {
            cardTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += distributionSpeed * Time.deltaTime;
            yield return null;
        }

        cardTransform.position = targetPosition;
    }
}
