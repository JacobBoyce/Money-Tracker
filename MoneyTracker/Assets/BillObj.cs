using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class BillObj : MonoBehaviour
{
    public TextMeshProUGUI bNameObj, bDueObj, progressText, totalAmountText, totalSavedText;
    public Image fillerObj, selectedImg;
    public float bAmount, bProgress;

    public void SelectedImgSwitch(bool var)
    {
        selectedImg.gameObject.SetActive(var);
    }

    public void PopulateBillInfo(string bname, int due, float amount, float progress)
    {
        bNameObj.text = bname;
        bDueObj.text = due.ToString();
        bAmount = amount;
        bProgress = progress;

        float tempPercentage = (progress / amount);
        
        if(tempPercentage*100 == 100)
        {
            progressText.text = (tempPercentage*100).ToString() + "%";
        }
        else
        {
            progressText.text = (tempPercentage*100).ToString("F1") + "%";
        }
        fillerObj.fillAmount = (tempPercentage);

        totalAmountText.text = amount.ToString("F2");
        totalSavedText.text = progress.ToString("F2");
        

    }

    public void SendToBillTracker()
    {
        GameObject.Find("Bill Tracker").SendMessage("FillEditBill",this);
    }
}