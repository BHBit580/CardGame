using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDistribution : MonoBehaviour
{
    [SerializeField] private List<GameObject> totalCards = new(); // Assign 52 card GameObjects in the Inspector
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private ListGameObjectSO cardsOnTable;
    [SerializeField] private AudioClip shufflingCard;
    [SerializeField] private AudioClip gameLoopMusic;

    [SerializeField] private Transform playerLocation , enemyLocation;
    [SerializeField] private float distributionSpeed = 5f;
    [SerializeField] private float timeDelay = 0.5f;
    
    void Start()
    {
        playerCards.data.Clear();
        enemyCards.data.Clear();
        cardsOnTable.data.Clear();
        foreach (Transform child in transform)
        {
            totalCards.Add(child.gameObject);
        }
        
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
        float startTime = Time.time;
        int batchSize = 2;
        SoundManager.instance.PlaySoundOneShot(shufflingCard);

        // Combine player and enemy cards
        List<GameObject> allCards = playerCards.data.Concat(enemyCards.data).ToList();

        // Iterate over cards in batches of four
        for (int i = 0; i < allCards.Count; i += batchSize)
        {
            // Get the current batch
            List<GameObject> currentBatch = allCards.Skip(i).Take(batchSize).ToList();

            // Move each card in the batch
            foreach (GameObject card in currentBatch)
            {
                Vector3 targetPosition = playerCards.data.Contains(card) ? playerLocation.position : enemyLocation.position;
                StartCoroutine(MoveCard(card.transform, targetPosition));
            }

            // Wait until enough time has passed
            yield return new WaitUntil(() => Time.time >= startTime + timeDelay);
            startTime = Time.time;
        }

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.StopMusic();
        SoundManager.instance.PlayMusicLoop(gameLoopMusic , 0.3f);
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
