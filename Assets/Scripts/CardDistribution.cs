using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDistribution : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>(); // Assign 52 card GameObjects in the Inspector
    public List<GameObject> playerCards;
    public List<GameObject> enemyCards;
    
    [SerializeField] private Transform playerLocation , enemyLocation;
    [SerializeField] private float distributionSpeed = 5f;
    [SerializeField] private float timeDelay = 0.5f;

    void Start()
    {
        RandomCardGenerator();
        StartCoroutine(DistributeCards());
    }

    private void RandomCardGenerator()
    {
        List<GameObject> tempDeck = new List<GameObject>(cards);
        
        while (tempDeck.Count > 0)
        {
            int randomIndex = Random.Range(0, tempDeck.Count);
            GameObject selectedCard = tempDeck[randomIndex];
            tempDeck.RemoveAt(randomIndex);

            if (playerCards.Count < enemyCards.Count)
            {
                playerCards.Add(selectedCard);
                selectedCard.layer = LayerMask.NameToLayer("PlayerCard");
            }
            else
            {
                enemyCards.Add(selectedCard);
            }
        }
    }
    
    IEnumerator DistributeCards()
    {
        foreach (GameObject card in playerCards)
        {
            StartCoroutine(MoveCard(card.transform, playerLocation.position));
            yield return new WaitForSeconds(timeDelay);
        }
        foreach (GameObject card in enemyCards)
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
