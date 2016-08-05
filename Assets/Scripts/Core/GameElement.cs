using UnityEngine;
using System.Collections;

public class GameElement : MonoBehaviour
{

    public GameInstanceManager GameManager
    {
        get
        {
            return GameInstanceManager.Instance;
        }
    }
}
