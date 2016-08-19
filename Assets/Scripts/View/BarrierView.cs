using UnityEngine;
using System.Collections;

public class BarrierView : MonoBehaviour
{

    public Transform[] sandbags;

	[Header("Smoke")]
	public GameObject DamageSmoke;
	[TooltipAttribute("Distance from origin")]
	public float ZDistance;
	[TooltipAttribute("Distance from origin on 1 side")]
	public float XDistance;
	public int SmokeCount;

	private Color original;	

    void Start()
    {		
		original = sandbags[0].GetChild(0).GetComponent<Renderer>().material.color;

        this.RegisterListener(EventID.OnBarrierLower, (sender, param) => LowerBarrier((int)param));
    }

	void ResetMaterials()
	{
		for (int i = 0; i < sandbags.Length; i++)
		{
			Transform bags = sandbags[i];

			for (int j = 0; j < bags.childCount; j++)
			{
				bags.GetChild(j).GetComponent<Renderer>().material.color = original;
			}
		}
	}

    private void LowerBarrier(int index)
    {
		StartCoroutine(Lower(sandbags[index]));
		SmokeParticle(sandbags[index].position);
    }

	private IEnumerator Lower(Transform barrier)
	{
		WaitForSeconds wait = new WaitForSeconds(0);
		float delta = 0;

		Material mat = barrier.GetComponent<Renderer>().material;

		Color end = new Color(original.r, original.g, original.b, 0);

		while (mat.color != end)
		{
			mat.color = Color.Lerp(original, end, delta);
			delta += 0.05f;
			yield return wait;		
		}
	}

	private void SmokeParticle(Vector3 bagPosition)
	{
		
	}
}
