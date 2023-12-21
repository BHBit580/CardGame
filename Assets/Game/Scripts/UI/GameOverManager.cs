using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private ListGameObjectSO playerCards;
    [SerializeField] private ListGameObjectSO enemyCards;
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 2f;
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

    public void OnClickReStartButton()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void OnClickHomeButton()
    {
        StartCoroutine(LoadLevel(0));
    }
    
    IEnumerator LoadLevel(int buildIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(buildIndex);
    }
}
