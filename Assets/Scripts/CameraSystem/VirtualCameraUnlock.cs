using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class VirtualCameraUnlock : VirtualCameraBase
{
    public void SetFollowTarget(Transform target)
    {
        VirtualCamera.Follow = target;
    }

    public void SetLookAt(Transform lookAt)
    {
        VirtualCamera.LookAt = lookAt;
    }
}