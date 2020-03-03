using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DialogueFunctions : MonoBehaviour
{
    public PlayableDirector timeline;
    // Start is called before the first frame update
    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTimeline()
    {
        Debug.Log("hweee");
        Debug.Log(timeline);
    }
}
