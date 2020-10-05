using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private List<AudioSource> audioPool = new List<AudioSource>();


    public const string BUTTON_CLICK = "click";
    public const string WRONG = "wrong";
    public const string RIGHT = "right";

    private const string path = "Sounds/";

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < 3; i++)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Audio";
            newObj.transform.parent = transform;
            audioPool.Add(newObj.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string audioName)
    {
        AudioSource audio = GetAudioFromPool();
        AudioClip clip = Resources.Load<AudioClip>(path + audioName);
        audio.clip = clip;
        audio.Play();
        StartCoroutine("DelayRecycle", audio);
    }

    private IEnumerator DelayRecycle(AudioSource audio)
    {
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = null;
    }



    private AudioSource GetAudioFromPool()
    {
        for (int i = 0; i < audioPool.Count; i++)
        {
            if (audioPool[i].clip == null) return audioPool[i];
        }

        GameObject newObj = new GameObject();
        newObj.name = "Audio";
        newObj.transform.parent = transform;
        AudioSource audio = newObj.AddComponent<AudioSource>();
        audioPool.Add(audio);

        return audio;
    }
}
