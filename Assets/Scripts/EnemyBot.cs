using UnityEngine;

public class EnemyBot : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CardDistribution cardDistribution;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float movementTimeDelay = 0.5f;
    [SerializeField] private Transform targetTransform1;

    private void Update()
    {
        if (gameManager.playerTurn == false)
        {
            GameObject card = cardDistribution.enemyCards[0];
            gameManager.MoveRotateCardFunc1(targetTransform1 , rotationAngle , rotationSpeed , movementTimeDelay , card , false);
            gameManager.playerTurn = true;
        }
    }
}
