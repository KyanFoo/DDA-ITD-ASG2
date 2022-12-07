using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    public bool redLock = false;
    public bool greenLock = false;
    public bool blueLock = false;
    public bool yellowLock = false;
    public GameObject TutorialDoor;
    public GameObject TutorialCongrats;

    public void RedUnlock()
    {
        redLock = true;
    }

    public void GreenUnlock()
    {
        greenLock = true;
    }

    public void YellowUnlock()
    {
        yellowLock = true;
    }

    public void BlueUnlock()
    {
        blueLock = true;
    }

    void Update()
    {
        if (redLock && greenLock && yellowLock && blueLock)
        {
            Destroy(TutorialDoor);
            TutorialCongrats.SetActive(true);
        }
    }
}
