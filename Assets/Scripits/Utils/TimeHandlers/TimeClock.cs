using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeClock : MonoBehaviour
{
    private const float REAL_SECONDS_PER_INGAME_DAY = 60f;
    private const int MONTHS_IN_YEAR = 12;

    private Transform clockHourHandTransform;
    private Transform clockMinuteHandTransform;
    private Text timeText;
    private Text dateText;

    private float timeSpeed = 1;
    private float day = .5f;
    private int month = 0;
    private int year = 0;
    private void Awake()
    {
        clockMinuteHandTransform = transform.Find("minuteHand");
        clockHourHandTransform = transform.Find("hourHand");
        timeText = transform.Find("timeText").GetComponent<Text>();
        dateText = GameObject.Find("dateText").GetComponent<Text>();
    }
    void Update()
    {
        day += Time.deltaTime / (REAL_SECONDS_PER_INGAME_DAY / timeSpeed);

        float dayNormalized = day % 1f;

        float rotationDegreesPerDay = 360f;
        clockHourHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay);

        float hoursPerDay = 24f;
        clockMinuteHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay * hoursPerDay);

        string hoursString = Mathf.Floor(dayNormalized * hoursPerDay).ToString("00");

        float minutesPerHour = 60f;
        string minutesString = Mathf.Floor(((dayNormalized * hoursPerDay) % 1) * minutesPerHour).ToString("00");

        timeText.text = hoursString + ":" + minutesString;
        if (day > GetDaysInMonth(month))
        {
            day = 0;
            month++;
        }
        if (month > MONTHS_IN_YEAR)
        {
            month = 1;
            year++;
        }
        dateText.text = Mathf.Floor(day).ToString("00") + "/" + month.ToString("00") + "/" + year.ToString("0000");
    }
    private int GetDaysInMonth(int currentMonth)
    {
        switch (currentMonth)
        {
            case 2:
                if (year % 4 == 0) return 29;
                return 28;
            case 4:
            case 6:
            case 9:
            case 11:
                return 30;
            default:
                return 31;
        }
    }
    public void SpeedUpTime(float speed)
    {
        timeSpeed += speed;
    }
    public void SlowDownTime(float speed)
    {
        timeSpeed -= speed;
    }
    public void SetTimeSpeed(float speed)
    {
        timeSpeed = speed;
    }
    public float GetTimeSpeed()
    {
        return timeSpeed;
    }
}
