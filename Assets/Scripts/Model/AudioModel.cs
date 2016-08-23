using UnityEngine;
using System.Collections;

public class AudioModel : MonoBehaviour {

    public AudioSource Ambient;
    public AudioSource GunShot;
    public AudioSource MissleLaunchSound;

    void Start()
    {
        this.RegisterListener(EventID.OnGameStart, (sender, param) => TurnOnAudio());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => TurnOffAudio());
        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => FireSound());
        this.RegisterListener(EventID.OnSpecialUsed , (sender, param) => LaunchMissleSound());
    }

    private void LaunchMissleSound()
    {
        MissleLaunchSound.Play();
    }

    private void FireSound()
    {
        GunShot.Play();
    }

    public void TurnOnAudio()
    {
        Ambient.enabled = true;
        Ambient.Play();
    }

    public void TurnOffAudio()
    {
        Ambient.Stop();
        Ambient.enabled = false;
    }
}
