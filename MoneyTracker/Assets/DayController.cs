using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayController : MonoBehaviour
{
    public CalendarController calController;
    public GameObject SelectedImg, paydayIndicatorPrefab, billIndicatorPrefab, infoPanel, paydayInd, billInd;
    public TMP_Text dayNum;
    // Start is called before the first frame update
    void Start()
    {
        calController = GameObject.Find("CalendarController / BG").GetComponent<CalendarController>();
        SelectedImg.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected()
    {
        calController.DaySelected(gameObject.GetComponent<DayController>());
    }

    public void ChangeImg(bool temp)
    {
        if(temp == true)
        {
            SelectedImg.GetComponent<Image>().enabled = true;
        }
        else
        {
            SelectedImg.GetComponent<Image>().enabled = false;
        }
    }

    public void InsertPayDayIndicator()
    {
        paydayInd = Instantiate(paydayIndicatorPrefab, this.gameObject.transform);
    }
    public void DeletePayDayIndicator()
    {
        Destroy(paydayInd); 
    }

    public void InsertBillIndicator()
    {
        Instantiate(billIndicatorPrefab, this.gameObject.transform);
    }

    public void EditDayNum(int num)
    {
        if(num < 0)
        {
            dayNum.text = "";
        }
        else
        {
            dayNum.text = num.ToString();
        }
    }

    public void ChangePayDayIndicator()
    {
        
    }
}