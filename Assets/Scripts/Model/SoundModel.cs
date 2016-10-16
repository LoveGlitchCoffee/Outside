using UnityEngine;
using System.Collections;

public class SoundModel : MonoBehaviour
{

    public AudioSource Cricket;
    public AudioSource Suspense;

    public AudioSource AmbientZombie;


    const float maxSuspenseVlm = 0.3f;
    public float VolumeDelta;

    void Start()
    {
        this.RegisterListener(EventID.SelectWeaponMenu, (sender, param) => StartCricket());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => StartCricket());
        this.RegisterListener(EventID.OnGameStart, (sender, param) => StopCricket());

        this.RegisterListener(EventID.OnGameStart, (sender, param) => PlayZombieAmbient());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => StopAmbientZombie());
        this.RegisterListener(EventID.OnGameProceed , (sender, param) => StopAmbientZombie());
        this.RegisterListener(EventID.OnPlayerWin , (sender, param) => StopAmbientZombie());

        this.RegisterListener(EventID.OnGameStart, (sender, param) => StartSuspense());
        this.RegisterListener(EventID.GoToEnd, (sender, param) => StopSuspense());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => StopSuspense());
        this.RegisterListener(EventID.SelectWeaponMenu, (sender, param) => StopSuspense());
    }

    private void StartCricket()
    {
        Cricket.Play();

        StartCoroutine(IncreaseSound(0.5f, Cricket));
    }

    private void StopCricket()
    {
        StartCoroutine(DecreaseSound(Cricket));
    }

    private void StartSuspense()
    {
        Suspense.Play();

        StartCoroutine(IncreaseSound(0.2f, Suspense));
    }

    private void StopSuspense()
    {
        StartCoroutine(DecreaseSound(Suspense));
    }

    IEnumerator IncreaseSound(float max, AudioSource audio)
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        while (audio.volume < max)
        {
            audio.volume += VolumeDelta;
            yield return wait;
        }
    }

    IEnumerator DecreaseSound(AudioSource audio)
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        while (audio.volume > 0)
        {
            audio.volume -= VolumeDelta;
            yield return wait;
        }

        audio.Stop();
    }

    public void PlayZombieAmbient()
    {
        AmbientZombie.enabled = true;
        AmbientZombie.Play();
    }

    public void StopAmbientZombie()
    {
        AmbientZombie.Stop();
        AmbientZombie.enabled = false;
    }


}
