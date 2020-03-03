using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public PlayableDirector timeline;
    public UnityEvent testing;

    private float counter_started = -1;
    private float counter_stopped = 0;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();

    }

    public void togglePlayableDirector()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        counter_started++;
        Debug.Log("counter started: " + counter_started);

    }

    public void stopTimeline()
    {
        Debug.Log("WHEEEEEE");
        Debug.Log("counter stopped: " + counter_stopped);
            
        if (Mathf.Approximately(counter_started - 1, counter_stopped))
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        counter_stopped++;

    }
}