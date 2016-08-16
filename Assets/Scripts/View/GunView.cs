using UnityEngine;
using System.Collections;

public class GunView : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem flash;

    void Awake()
    {
        anim = GetComponent<Animator>();
        flash = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
	
	void Start () {
	    this.RegisterListener(EventID.OnPlayerFire, (sender, param) => Recoil());
        this.RegisterListener(EventID.OnReload, (sender, param) => Reload());
	}

    private void Recoil()
    {
        anim.SetBool("Fired", true);
        flash.Play();
    }

    private void Reload()
    {
        anim.SetBool("Reload", true);
    }
}
