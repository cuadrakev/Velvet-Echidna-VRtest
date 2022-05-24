using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class RayObjectCaller : MonoBehaviour
{
    public void OnPressUp(ViveInputVirtualButton.OutputEventArgs args)
    {
        Transform hand;
        if (args.senderObj.inputs[0].viveRole.roleValue == (int)HandRole.RightHand)
         hand = args.senderObj.gameObject.transform.Find("Right");
       else
          hand = args.senderObj.gameObject.transform.Find("Left");
        ReticlePoser reticle = hand.Find("Reticle").gameObject.GetComponent<ReticlePoser>();
        GameObject hitObject = reticle.hitTarget;
        BasicGrabbable grabbableComponent = hitObject.GetComponent<BasicGrabbable>();
        if(grabbableComponent != null)
        {
            hitObject.transform.position = hand.Find("PoseTracker").position;
        }
    }
}
