using UnityEngine;
using System.Collections;

public class KillModel : MonoBehaviour
{
    public KillTextBehaviour text;

    [Header("Audio")]
    AudioSource audio;
    public AudioClip DoubleSound;
    public AudioClip MultiSound;

    [Header("Particle")]
    public GameObject Impact;


    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }


    void Start()
    {
        this.RegisterListener(EventID.OnDoubleKill, (sender, param) => PlayDouble());
        this.RegisterListener(EventID.OnMultiKill, (sender, param) => PlayMulti());
        this.RegisterListener(EventID.OnEnemyDie , (sender, param) => ImpactEffect((Collision) param));
        this.RegisterListener(EventID.OnEnemyDie , (sender, param) => Debug.Log("hit enemy"));

        Debug.Log("impact prefab: " + Impact);
    }

    private void ImpactEffect(Collision col)
    {
        Debug.Log("col " + col.transform.position);
        ContactPoint cp = col.contacts[0];        

        Quaternion rot = Quaternion.FromToRotation(Vector3.down, cp.normal);
        Vector3 pos = cp.point;

        var imp = PoolManager.Instance.spawnObject(Impact, pos, rot);
        imp.GetComponent<ImpactParticleBehaviour>().Play(rot);
    }


    private void PlayDouble()
    {
        text.Double();

        PlaySound(false);
    }

    private void PlayMulti()
    {
        text.Multi();

        PlaySound(true);
    }

    private void PlaySound(bool multi)
    {
        if (audio.isPlaying)
            audio.Stop();

        if (multi)
            audio.clip = MultiSound;
        else
            audio.clip = DoubleSound;

        audio.Play();
    }
}
