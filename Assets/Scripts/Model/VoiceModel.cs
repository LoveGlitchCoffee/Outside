using UnityEngine;
using System.Collections;
using Random = System.Random;

public class VoiceModel : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip ReloadClip;
    public AudioClip[] VoiceClips;

    private int noOfVoices;
    private Random voiceGen;
    private int previous;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Start ()
	{
	    noOfVoices = VoiceClips.Length;

        voiceGen = new Random((int) Time.time);

        this.RegisterListener(EventID.OnReload, (sender, param) => ReloadVoice());
        
        InvokeRepeating("PlayRandomVoiceLine", 20, 20);
	}

    private void ReloadVoice()
    {
        if (audio.isPlaying)
            return;

        audio.clip = ReloadClip;
        audio.Play();
    }

    private void PlayRandomVoiceLine()
    {
        if (audio.isPlaying)
            return;

        int next = voiceGen.Next(noOfVoices);

        while (next == previous)
        {
            next = voiceGen.Next(noOfVoices);
        }        

        audio.clip = VoiceClips[next];
        audio.Play();

        previous = next;
    }
	    
}
