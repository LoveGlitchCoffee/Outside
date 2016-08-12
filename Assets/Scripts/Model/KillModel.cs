using UnityEngine;
using System.Collections;

public class KillModel : MonoBehaviour
{
    public KillTextBehaviour text;    

    void Awake()
    {
        
    }


    void Start ()
    {
        this.RegisterListener(EventID.OnDoubleKill, (sender, param) => PlayDouble());
        this.RegisterListener(EventID.OnMultiKill, (sender, param) => PlayMulti());
    }


    private void PlayDouble()
    {
        text.Double();
    }

    private void PlayMulti()
    {
        text.Multi();
    }
}
