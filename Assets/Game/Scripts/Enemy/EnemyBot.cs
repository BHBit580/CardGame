using System.Collections;
using UnityEngine;

public class EnemyBot : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CardChecker cardChecker;
    [SerializeField] private AudioClip cardPutSound;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private ListGameObjectSO cardsOnTable;
    
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float endRotationAngleDelta = 60f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float movementTimeDelay = 0.5f;
    [SerializeField] private Transform targetTransform1;
    [SerializeField] private Transform targetTransform2;

    private GameObject card;
    private bool onlyOnce = true;
    private void Update()
    {
        if (gameManager.playerTurn == false && onlyOnce)
        {
            card = enemyCards.data[enemyCards.data.Count-1];
            StartCoroutine(MoveRotatePosition1(targetTransform1.position));
            onlyOnce = false;
        }
    }
    
    IEnumerator MoveRotatePosition1(Vector3 targetTransform1Position)
    {
        Quaternion initialRotation = card.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(
            card.transform.eulerAngles.x,
            card.transform.eulerAngles.y,
            card.transform.eulerAngles.z + rotationAngle
        );

        Vector3 initialPosition = card.transform.position;
        
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            card.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            card.transform.position = Vector3.Lerp(initialPosition, targetTransform1Position, elapsedTime);

            elapsedTime += rotationSpeed * Time.deltaTime;
            yield return null;
        }

        card.transform.rotation = targetRotation;
        card.transform.position = targetTransform1Position;

        
        if (Vector3.Distance(card.transform.position, targetTransform1Position) < 0.1f)
        {
            yield return new WaitForSeconds(movementTimeDelay);
            StartCoroutine(MoveRotatePosition2(targetTransform2.position));
        }
    }

    IEnumerator MoveRotatePosition2(Vector3 targetPosition2)
    {
        Vector3 initialPosition = card.transform.position;
        Quaternion initialRotation = card.transform.rotation;
    
        // Use a single rotation axis (e.g., Quaternion.AngleAxis) to avoid gimbal lock
        Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(-endRotationAngleDelta, endRotationAngleDelta), Vector3.up);
        Quaternion targetRotation = initialRotation * randomRotation;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            card.transform.position = Vector3.Lerp(initialPosition, targetPosition2, elapsedTime);
            card.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += movementSpeed * Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetPosition2;
        
        if (cardChecker.CheckCardMatching(card))
        {
            cardsOnTable.data.Add(card);
            enemyCards.data.RemoveAt(enemyCards.data.Count-1);
            yield return StartCoroutine(cardChecker.MoveCardToEnemyLocation());
            yield return new WaitForSeconds(1f);
            gameManager.playerTurn = false;
        }
        else
        {
            SoundManager.instance.PlayEffectOneShot(cardPutSound);
            cardsOnTable.data.Add(card);
            enemyCards.data.RemoveAt(enemyCards.data.Count-1);
            gameManager.playerTurn = true;
        }
        
        onlyOnce = true;
        targetTransform2.position = new Vector3(targetTransform2.position.x , targetTransform2.position.y , targetTransform2.position.z - 0.01f);
    }
}
