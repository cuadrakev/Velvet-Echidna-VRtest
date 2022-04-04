using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VR_UIController : MonoBehaviour
{
    public GameObject rightPointer;
    public GameObject leftPointer;
    public GameObject menuCanvas;
    public CustomDeviceHeight deviceHeight;

    private bool menuOpen = false;
    private bool rightPointerActive = false;
    private bool leftPointerActive = false;

    protected  virtual void Start()
    {
        rightPointer.SetActive(rightPointerActive);
        leftPointer.SetActive(leftPointerActive);
        menuCanvas.SetActive(menuOpen);
    }


    protected virtual void LateUpdate()
    {
        bool updateNeeded = false;

        if(ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu))
        {
            updateNeeded = true;
            rightPointerActive = !rightPointerActive;
            menuOpen = !menuOpen;
        }
        if(ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.Menu))
        {
            updateNeeded = true;
            leftPointerActive = !leftPointerActive;
            menuOpen = !menuOpen;
        }

        if(updateNeeded)
        {
            menuCanvas.SetActive(menuOpen);
            if (menuOpen)
            {
                rightPointer.SetActive(rightPointerActive);
                leftPointer.SetActive(leftPointerActive);
                
                Vector3 position = Camera.main.transform.position;
                Vector3 forwardVector = Camera.main.transform.forward;

                float angle = Quaternion.Angle(Quaternion.AngleAxis(0, new Vector3(0, 1, 0)), Camera.main.transform.rotation);
                if (Camera.main.transform.rotation.w < 0.0)
                    angle = -angle;
                Quaternion rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));

                position += forwardVector * 3;
                menuCanvas.transform.position = position;
                menuCanvas.transform.rotation = rotation;
            }
            else
            {
                rightPointerActive = false;
                leftPointerActive = false;
                rightPointer.SetActive(false);
                leftPointer.SetActive(false);
            }
        }
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ChangeHeight(Dropdown dropdown)
    {
        float height = float.Parse(dropdown.options[dropdown.value].text);
        deviceHeight.height = height / 100.0f;
    }
}
