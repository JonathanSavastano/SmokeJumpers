using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float elapsedTime;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");
        timerText.text = "Time: " + minutes + ":" + seconds;
    }
}
