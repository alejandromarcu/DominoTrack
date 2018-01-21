using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class EntitlementCheck : MonoBehaviour
{

    void Start()
    {
        Oculus.Platform.Core.Initialize("1641825905840347");
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementChecked);
    }

    void EntitlementChecked(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("Entitlement failed: " + msg);
            UnityEngine.Application.Quit();
        }
    }
}
