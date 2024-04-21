using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField][Range(0,900)] private float timeRemaining;

    void Update()
    {
        if (timeRemaining >= 0f)
        {
            timeRemaining -= Time.deltaTime;

            UpdateTimerText();
        }
        else
        {
            GameManager.Instance.ShowTimeOverPanel();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}