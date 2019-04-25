using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.VR;
 
public class Headset : MonoBehaviour
{
    void FixedUpdate() 
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject camera = GameObject.Find("Main Camera");
            if (camera != null)
            {
                transform.localRotation = Quaternion.Inverse(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye));
                transform.localPosition = -UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
            }
        }
    }
}
 