using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CalendarController : MonoBehaviour
{
    public List<DayController> days = new List<DayController>();
    private DayController selected;
    private int curDayOfWeekNum, firstOfMonthDayOfWeekNum, daysInMonth;
    public TMP_Text monthTitle;
    public GameObject dayDetails;
    // Start is called before the first frame update
    void Start()
    {
        // add all days to list
        foreach(Transform t in gameObject.transform)
        {
            days.Add(t.GetComponent<DayController>());
        }

        //find first day of month then change numbers of all days
        GetDayOfWeekNumber();
        firstOfMonthDayOfWeekNum = GetFirstDayOfMonth(curDayOfWeekNum, System.DateTime.Today.Day);

        daysInMonth = System.DateTime.DaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month);
        monthTitle.text = DateTime.Now.ToString("MMMM");

        int firstDay = 1;
        int offset = firstOfMonthDayOfWeekNum;
        
        foreach(DayController day in days)
        {
            if(offset > 0)
            {
                day.GetComponent<DayController>().EditDayNum(-1);
                offset--;
            }
            else if(firstDay <= daysInMonth)
            {
                day.GetComponent<DayController>().EditDayNum(firstDay);
                firstDay++;
            }
            else
            {
                day.GetComponent<DayController>().EditDayNum(-1);
            }
        }
    }

    public int GetFirstDayOfMonth(int dayOfWeek, int todaysDayOfMonth)
    {

        int i = todaysDayOfMonth;
        while(i>1)
        {
            dayOfWeek--;
            if(dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            i--;
        }
         return dayOfWeek;
    }

    public void GetDayOfWeekNumber()
    {
        if(System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            curDayOfWeekNum = 0;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)
        {
            curDayOfWeekNum = 1;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
        {
            curDayOfWeekNum = 2;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
        {
            curDayOfWeekNum = 3;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
        {
            curDayOfWeekNum = 4;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            curDayOfWeekNum = 5;
        }
        else if(System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            curDayOfWeekNum = 6;
        }
    }

    public void DaySelected(DayController day)
    {
        // select and deslect days
        foreach(DayController d in days)
        {
            if(d == day)
            {
                d.ChangeImg(true);
                selected = d;
            }
            else
            {
                d.ChangeImg(false);
            }
        }

        //activate day details
        dayDetails.SetActive(true);
        //setup day details
    }

    public void AddPaydayToDay()
    {
        selected.InsertPayDayIndicator();
    }
}
