using UnityEngine;
using System.Collections;

public class KillVoiceModel : MonoBehaviour
{
    

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
        
    }

    private void PlayMulti()
    {
        
    }
}
