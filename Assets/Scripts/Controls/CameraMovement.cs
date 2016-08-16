using UnityEngine;
using System.Collections;

public class CameraMovement : GameElement
{
    private Vector2 mouseLook;
    private Vector2 smoothV;

    public float sensitivity;
    public float smoothing; // kinda more like reverse-sensitivity

    private GameObject player;
    public float RotationSpeed;

    void Start()
    {
        player = transform.parent.gameObject;

        this.RegisterListener(EventID.OnGameStart, (sender, param) => Activate());
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => Deactivate());
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => TurnToFace((Vector3) param));

        Deactivate();
    }

    private void TurnToFace(Vector3 zombie)
    {
        zombie = new Vector3(zombie.x, zombie.y + 1, zombie.z);

        Vector3 direction = (zombie - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        StartCoroutine(Turn(lookRotation));
    }

    private IEnumerator Turn(Quaternion lookRotation)
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        float time = 0;

        while (!WithinDistance(lookRotation, transform.rotation) || time < 2)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*RotationSpeed);
            yield return wait;
            time += Time.deltaTime;
        }

        
        this.PostEvent(EventID.OnGameEnd);
    }

    private bool WithinDistance(Quaternion lookRotation, Quaternion rotation)
    {
        float xDelta = lookRotation.x - rotation.x;
        float yDelta = lookRotation.y - rotation.y;
        float zDelta = lookRotation.z - rotation.z;

        float threshold = 0.002f;

        return xDelta < threshold && yDelta < threshold && zDelta < threshold;
    }

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity*smoothing, sensitivity*smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f/smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f/smoothing);

        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        //transform.GetChild(0).localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}