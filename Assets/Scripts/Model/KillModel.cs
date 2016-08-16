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
    }

    private void ImpactEffect(Collision col)
    {
        ContactPoint cp = col.contacts[0];

        Quaternion rot = Quaternion.FromToRotation(Vector3.down, cp.normal);
        Vector3 pos = cp.point;

        Debug.Log("Recieved die, rotation " + rot.eulerAngles);

        var imp = PoolManager.Instance.GetFromPool(Impact, pos, rot);
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
