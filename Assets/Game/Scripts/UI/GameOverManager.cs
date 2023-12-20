using UnityEngine;
using UnityEngine.Serialization;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private GameObject[] objectsToBeOff;
    [SerializeField] private AudioClip gameOverSound;

    private bool onlyOnce = true;
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if ((playerCards.data.Count == 0 || enemyCards.data.Count == 0) && onlyOnce)
        {
            onlyOnce = false;
            SoundManager.instance.StopMusic();
            SoundManager.instance.StopEffect();
            foreach (GameObject obj in objectsToBeOff)
            {
                obj.SetActive(false);
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            SoundManager.instance.PlayEffectOneShot(gameOverSound);
        }
    }
}
