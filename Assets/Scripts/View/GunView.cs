using UnityEngine;
using System.Collections;

public class GunView : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
	
	void Start () {
	    this.RegisterListener(EventID.OnPlayerFire, (sender, param) => Recoil());
        this.RegisterListener(EventID.OnReload, (sender, param) => Reload());
	}

    private void Recoil()
    {
        anim.SetBool("Fired", true);
    }

    private void Reload()
    {
        anim.SetBool("Reload", true);
    }
}
