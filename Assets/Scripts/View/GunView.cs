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
	
	protected virtual void Start () {
	    this.RegisterListener(EventID.OnPlayerFire, (sender, param) => Recoil());
        this.RegisterListener(EventID.OnReload, (sender, param) => Reload());
	}

    protected void Recoil()
    {
        Debug.Log("set fire true");
        //anim.SetBool("Fired", true);
        flash.Play();
    }

    protected void Reload()
    {
        anim.SetBool("Reload", true);
    }
}
