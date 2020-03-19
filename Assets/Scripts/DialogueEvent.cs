using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "TestScriptableObject")]
public class TestScriptableObject : ScriptableObject
{
    public AudioClip[] audioClips;
    public void PlayAudioClip(int i)
    {
        AudioSource.PlayClipAtPoint(audioClips[i], Vector3.zero);
    }
    public void loadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}