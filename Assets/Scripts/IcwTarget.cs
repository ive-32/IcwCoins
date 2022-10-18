using System.Collections.Generic;
using UnityEngine;

public class IcwTarget : MonoBehaviour
{

    private void OnCollisionStay2D(Collision2D collision)
    {
        IWalker otherObject = null;
        IRoute thisRoute = null;
        this.transform.parent.TryGetComponent<IRoute>(out thisRoute);
        if (thisRoute == null || !thisRoute.inProgress) return;
        collision.gameObject.TryGetComponent<IWalker>(out otherObject);
        if (otherObject == thisRoute.walker && otherObject.TargetReached)
        {
            thisRoute.Completed = true;
        }

    }

}
