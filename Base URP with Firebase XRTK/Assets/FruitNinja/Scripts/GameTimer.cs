using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GameTimer : MonoBehaviour
{
    //Set the first game timer at the Inspector.
    //Set the second timer after the reset is at below "else" timeValue.
    public float timeValue = 90;
    public TextMeshProUGUI timeText;
    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            //**Here**//
            timeValue = 30;
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    //Reset the game not only the score but also the timing.
    public void RestartTime()
    {
        timeValue = 30;
        DisplayTime(timeValue);
    }
}
