﻿using UnityEngine;
using System.Collections;

public class AudioModel : MonoBehaviour {

    public AudioSource Ambient;
    public AudioSource GunShot;
    public AudioSource Shotgun;
    public AudioSource MissleLaunchSound;
    public AudioSource HitEffect;

    void Start()
    {
        this.RegisterListener(EventID.OnGameStart, (sender, param) => TurnOnAudio());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => TurnOffAudio());
        this.RegisterListener(EventID.OnGameProceed , (sender, param) => TurnOffAudio());
        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => FireSound());
        this.RegisterListener(EventID.OnPlayerFireRight , (sender, param) => FireSound());
        this.RegisterListener(EventID.OnPlayerFireLeft , (sender, param) => FireSound());
        this.RegisterListener(EventID.OnShotgunFire , (sender, param) => ShotgunSound());
        this.RegisterListener(EventID.OnMissleLaunch , (sender, param) => LaunchMissleSound());
        this.RegisterListener(EventID.OnEnemyDie , (sender, param) => HitEffectSound());
    }

    private void HitEffectSound()
    {
        HitEffect.Play();
    }

    private void LaunchMissleSound()
    {
        MissleLaunchSound.Play();
    }

    private void FireSound()
    {
        GunShot.Play();
    }

    private void ShotgunSound()
    {
        Shotgun.Play();
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
