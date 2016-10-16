using UnityEngine;
using System.Collections;

public class AudioModel : MonoBehaviour {

    public AudioSource GunShot;
    public AudioSource Shotgun;
    public AudioSource MissleLaunchSound;
    public AudioSource HitEffect;

    void Start()
    {
        
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
}
