using UnityEngine;
using TMPro;

public class EnemyScore : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO enemyCards;
    private TextMeshProUGUI enemyScore;

    private void Start()
    {
        enemyScore = GetComponent<TextMeshProUGUI>();
        enemyScore.text = null;
    }

    private void Update()
    {
        enemyScore.text = "Score:" + enemyCards.data.Count;
    }
}
