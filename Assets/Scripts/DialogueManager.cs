using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public PlayableDirector timeline;
    public UnityEvent testing;

    private float counter_started = -1;
    private float counter_stopped = 0;
    private GameObject fadeCanvas;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        fadeCanvas = transform.Find("FaderCanvas").gameObject;

    }
    public void togglePlayableDirector()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        counter_started++;

    }

    public void stopTimeline()
    {
            
        if (Mathf.Approximately(counter_started - 1, counter_stopped))
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        counter_stopped++;

    }

    public void loadScene(int i)
    {
        SceneManager.LoadScene(i);
        Debug.Log("LOADING SCENE...");
    }

    public void fade()
    {
        Debug.Log("fading...");
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        for (float ft = 1f; ft >= -0.1; ft -= 0.1f)
        {
            fadeCanvas.GetComponent<CanvasGroup>().alpha = 1 - ft;
            yield return null;
        }
    }

}