using UnityEngine;
using System.Collections;

public class RightGunView : GunView {

    protected override void Start()
    {
        this.RegisterListener(EventID.OnPlayerFireRight, (sender, param) => Recoil());
        this.RegisterListener(EventID.OnReload , (sender, param) => Reload());
    }
}
