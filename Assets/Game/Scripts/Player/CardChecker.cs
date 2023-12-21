using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CardChecker : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO cardsOnTable;
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private AudioClip cardsPuttingEffectSound;
    [SerializeField] private Transform playerLocation;
    [SerializeField] private Transform enemyLocation;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float timeDelay = 0.5f;

    public bool CheckCardMatching(GameObject card)
    {
        string topCardName = (cardsOnTable.data.Count !=0) ? cardsOnTable.data[cardsOnTable.data.Count - 1].name : "null";
        return AreNumericPartsEqual(card.name, topCardName);
    }
    
    public IEnumerator MoveCardToPlayerLocation()
    {
        SoundManager.instance.PlayEffectLoop(cardsPuttingEffectSound);
        cardsOnTable.data = cardsOnTable.data.OrderBy(x => UnityEngine.Random.value).ToList();

        // Determine the number of cards to move in each batch
        int batchSize = 4;
        for (int i = 0; i < cardsOnTable.data.Count; i += batchSize)
        {
            List<GameObject> currentBatch = cardsOnTable.data.Skip(i).Take(batchSize).ToList();
            foreach (GameObject card in currentBatch)
            {
                card.layer = LayerMask.NameToLayer("PlayerCard");
                StartCoroutine(MoveCard(card, playerLocation.position));
            }
            yield return new WaitForSeconds(timeDelay);
        }
        
        playerCards.data.InsertRange(0 , cardsOnTable.data);
        cardsOnTable.data.Clear();
        SoundManager.instance.StopEffect();
    }

    
    public IEnumerator MoveCardToEnemyLocation()
    {
        SoundManager.instance.PlayEffectLoop(cardsPuttingEffectSound);
        cardsOnTable.data = cardsOnTable.data.OrderBy(x => UnityEngine.Random.value).ToList();
        
        int batchSize = 4;
        
        for (int i = 0; i < cardsOnTable.data.Count; i += batchSize)
        {
            List<GameObject> currentBatch = cardsOnTable.data.Skip(i).Take(batchSize).ToList();
            foreach (GameObject card in currentBatch)
            {
                StartCoroutine(MoveCard(card, enemyLocation.position));
            }
            yield return new WaitForSeconds(timeDelay);
        }
        
        enemyCards.data.InsertRange(0 , cardsOnTable.data);
        cardsOnTable.data.Clear();  
        SoundManager.instance.StopEffect();
    }

    
    IEnumerator MoveCard(GameObject card, Vector3 targetPosition)
    {
        Vector3 initialPosition = card.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            card.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += speed * Time.deltaTime;
            yield return null;
        }
        card.transform.position = targetPosition;
        card.transform.rotation = Quaternion.Euler(-90f, -90f, -90f);
    }

    public bool AreNumericPartsEqual(string str1, string str2)
    {
        string numericPart1 = ExtractNumericPart(str1);
        string numericPart2 = ExtractNumericPart(str2);

        // Compare the numeric parts
        return numericPart1 == numericPart2;
    }

    public string ExtractNumericPart(string input)
    {
        // Use regular expression to match numeric part
        Match match = Regex.Match(input, @"\d+");

        // Check if a numeric part is found
        if (match.Success)
        {
            return match.Value;
        }

        return string.Empty; // No numeric part found
    }

}
