using UnityEngine;
using System.Collections;
using Random = System.Random;

public class VoiceModel : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip ReloadClip;
    public AudioClip SpecialClip;
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
        this.RegisterListener(EventID.OnSpecialUsed , (sender, param) => SpecialVoice());
        this.RegisterListener(EventID.OnGameStart , (sender, param) => StartVoice());
        this.RegisterListener(EventID.OnGameEnd , (sender, param) => EndVoice());
        this.RegisterListener(EventID.OnGameProceed , (sender, param) => EndVoice());
	}

    private void StartVoice()
    {
        InvokeRepeating("PlayRandomVoiceLine", 20, 20);        
    }

    private void EndVoice()
    {
        CancelInvoke("PlayRandomVoiceLine");
        audio.Stop();
    }

    private void ReloadVoice()
    {
        if (audio.isPlaying)
            return;

        audio.clip = ReloadClip;
        audio.Play();

        StartCoroutine(waitTillVoiceFinish());
    }

    private void SpecialVoice()
    {
        if (audio.isPlaying)
            audio.Stop();

        audio.clip = SpecialClip;
        audio.Play();

        StartCoroutine(waitTillVoiceFinish());
    }

    IEnumerator waitTillVoiceFinish()
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        while (audio.isPlaying)
            yield return wait;

        InvokeRepeating("PlayRandomVoiceLine", 20, 20);
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
