using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float endRotationAngleDelta = 60f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float movementTimeDelay = 0.5f;
    
    [SerializeField] private AudioClip cardPutSound;
    [SerializeField] private Transform targetTransform1;
    [SerializeField] private Transform targetTransform2;
    [SerializeField] private CardChecker cardChecker;
    [SerializeField] private ListGameObjectSO cardsOnTable;
    [SerializeField] private ListGameObjectSO playerCards;
    
    public bool playerTurn = true;
    private bool cardMoving;
    private Vector3 cardInitialRotation = new Vector3(-90, -90, -90);
    private GameObject card;
    
    
    private void OnTouchInput(InputValue value)
    {
        if (value.Get<float>() > 0.5f)
        {
            if (playerTurn && !cardMoving)
            {
                CastRay();
            }
        }
    }

    void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity , LayerMask.GetMask("PlayerCard")))       //Collider hit something
        {
            card = playerCards.data[playerCards.data.Count-1];
            cardMoving = true;
            StartCoroutine(MoveRotatePosition1(targetTransform1.position));
        }
    }
    
    IEnumerator MoveRotatePosition1(Vector3 targetTransform1Position)
    {
        Quaternion initialRotation = Quaternion.Euler(cardInitialRotation);
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0, rotationAngle);

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
            cardMoving = false;
            yield return StartCoroutine(cardChecker.MoveCardToPlayerLocation());
            playerTurn = true; 
            playerCards.data.Remove(card);
        }
        else
        {
            SoundManager.instance.PlayEffectOneShot(cardPutSound);
            cardsOnTable.data.Add(card);
            cardMoving = false;
            playerCards.data.Remove(card);
            card.layer = LayerMask.NameToLayer("Default");
            
            targetTransform2.position = new Vector3(targetTransform2.position.x, targetTransform2.position.y,
                targetTransform2.position.z - 0.01f);
            playerTurn = false;                        //It's now enemy's turn player card has reached targetPosition2
        }
        
    }
    

}
