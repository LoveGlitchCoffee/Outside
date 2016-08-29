using UnityEngine;
using System.Collections;

public class CommonFunctions {

	static Vector2 viewportCrosshair = new Vector2(0.5f,0.56f);

	public static Vector3 RaycastBullet(Transform gun, float bulletSpeed)
	{
		Ray ray = Camera.main.ViewportPointToRay(viewportCrosshair);
		RaycastHit hit;

		Physics.Raycast(ray, out hit);

		Debug.DrawLine(ray.origin, hit.point, Color.red, 10);

		Vector3 contact = hit.point;

		Vector3 direction = contact - gun.position;

		float distance = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2) + Mathf.Pow(direction.z, 2));

		float multiply = bulletSpeed / distance;

		Vector3 bulletForce = direction * multiply;

		return bulletForce;
	}
}
