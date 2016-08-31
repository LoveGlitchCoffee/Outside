using UnityEngine;
using System.Collections;

public class MissleView : GameElement
{

    public GameObject Missle;
    public ParticleSystem MissleFlash;   

	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	} 

	void Start()
	{
		this.RegisterListener(EventID.OnSpecialUsed , (sender, param) => Launch((float) param));		
	}


    private void Launch(float MissleSpeed)
    {
		StartCoroutine(LaunchMissleCoroutine(MissleSpeed));        
    }

	private IEnumerator LaunchMissleCoroutine(float MissleSpeed)
	{
		yield return StartCoroutine(GameManager.view.Weapon.LowerWeapon());

		anim.SetBool("Launch", true);

		yield return new WaitForSeconds(1); // for missle anim

		this.PostEvent(EventID.OnMissleLaunch);
		SpawnMissle(CommonFunctions.RaycastBullet(transform, MissleSpeed));		

		anim.SetBool("Launch", false);

		StartCoroutine(GameManager.view.Weapon.RaiseWeapon());
	}		

	private void SpawnMissle(Vector3 direction)
	{
		MissleFlash.transform.position = transform.position + transform.TransformDirection(new Vector3(0, 0, 0.5f));
        MissleFlash.Play();

        var mis = Missle.GetComponent<MissleBehaviour>();
        mis.transform.position = transform.position + transform.TransformDirection(new Vector3(0, 0, 0.5f));
        mis.transform.rotation = transform.rotation;
        mis.SetUp(direction);
	}
}
