using UnityEngine;
using System.Collections;

public class ShotgunView : MonoBehaviour
{

    private Animator anim;

    private ParticleSystem[] muzzleFlashes;

    void Awake()
    {
		anim = GetComponent<Animator>();
        muzzleFlashes = new ParticleSystem[4];        
    }

    void Start()
    {
		for (int i = 0; i < 4; i++)
        {
			Debug.Log(transform.GetChild(i));
            muzzleFlashes[i] = transform.GetChild(i).GetChild(0).GetComponent<ParticleSystem>();
        }

        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => Recoil());
        this.RegisterListener(EventID.OnReload, (sender, param) => Reload());

    }

    protected void Recoil()
    {
        anim.SetBool("Fired", true);

        for (int i = 0; i < 4; i++)
        {
            muzzleFlashes[i].Play();
        }
    }

    protected void Reload()
    {
        anim.SetBool("Reload", true);
    }

}
