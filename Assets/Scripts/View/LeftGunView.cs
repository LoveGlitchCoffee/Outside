using UnityEngine;
using System.Collections;

public class LeftGunView : GunView
{

    protected override void Start()
    {
        this.RegisterListener(EventID.OnPlayerFireLeft, (sender, param) => Recoil());
    }
}
