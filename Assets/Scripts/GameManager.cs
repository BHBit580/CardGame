using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float endRotationAngleDelta = 60f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float movementTimeDelay = 0.5f;
    [SerializeField] private Transform targetTransform1;
    [SerializeField] private Transform targetTransform2;

    public bool playerTurn = true;
    private bool cardMoving = false;
    private GameObject card;
    private List<GameObject> cardAtTable = new();
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerTurn && !cardMoving)
        {
            CastRay();
        }
    }

    void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity , LayerMask.GetMask("PlayerCard")))       //Collider hit something
        {
            card = hit.collider.gameObject;
            if (!cardAtTable.Contains(card))
            {
                cardAtTable.Add(card);
                cardMoving = true;
                MoveRotateCardFunc1(targetTransform1 , rotationAngle , rotationSpeed , movementTimeDelay , card , true);
            }
        }
    }

    public void MoveRotateCardFunc1(Transform targetTransform1 , float rotationAngle , float rotationSpeed , float movementTimeDelay , GameObject card , bool isPlayer)
    {
        StartCoroutine(MoveAndRotateCard(targetTransform1.position , rotationAngle , rotationSpeed , movementTimeDelay , card , isPlayer));
    }

    IEnumerator MoveAndRotateCard(Vector3 targetTransform1Position , float rotationAngle , float rotationSpeed , float movementTimeDelay , GameObject card , bool isPlayer)
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
            StartCoroutine(MoveToTargetPosition2(targetTransform2.position , movementSpeed , endRotationAngleDelta , card  , isPlayer));
        }
    }

    IEnumerator MoveToTargetPosition2(Vector3 targetPosition2 , float movementSpeed , float endRotationAngleDelta , GameObject card , bool isPlayer)
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
        
        targetPosition2 = new Vector3(targetPosition2.x, targetPosition2.y, targetPosition2.z - 0.01f);

        
        if (isPlayer)
        {
            playerTurn = false; //It's now enemy's turn player card has reached targetPosition2
            cardMoving = false;
        }
    }

}
