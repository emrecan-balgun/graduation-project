using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int second=60;
    public Text timer;
    private TimeSpan time;
    private void Start()
    {
        StartCoroutine(StartCountdown(second));

    }

 
    public IEnumerator StartCountdown(int second)
    {
        
        while (second > 0)
        {
            int min = second / 60;
            timer.text = (min+":" + second%60);
            yield return new WaitForSeconds(1.0f);
            second--;
        }

        if (second == 0)
        {
            timer.text = "0:0";
            Time.timeScale = 0;
        }
    }
}
