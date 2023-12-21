using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 2f;

    public void OnClickStartButton()
    {
        StartCoroutine(StartGame());
    }
    
    IEnumerator StartGame()
    {
        animator.SetTrigger("Start");
        SoundManager.instance.StopMusic();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}