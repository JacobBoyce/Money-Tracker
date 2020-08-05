using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace QuantumTek.EncryptedSave
{

public class BillController : MonoBehaviour
{
    public GameObject saveObj, billObjPrefab, billContainer, tempBill, addEditPanel, dateText;
    public TMP_InputField bNameIF, bTAmountIF, bAmountSavedIF;
    public TMP_Dropdown bDueDD, bSortDD;
    public TMP_Text errorMsg, allTotalAmount, allBillAmount;
    private float allTotalFloat, allBillTotal;
    public bool errorCheck, editCheck;
    public BillObj selectedBill;
    [Space]
    public List<BillObj> bills = new List<BillObj>();
    public List<string[]> billz = new List<string[]>();
    public string[] tempBillSaveArray;
    
    // Start is called before the first frame update
    void Start()
    {
        dateText.GetComponent<TMP_Text>().text = System.DateTime.Now.Month + "/" + System.DateTime.Now.Day + "/" + System.DateTime.Now.Year;
        //Init vars
        tempBillSaveArray = new string[4];
        editCheck = false;
        //load bills into list
        if(ES_Save.Exists("UserData.src"))
        {
            billz = ES_Save.Load<List<string[]>>("Userdata.src");
            foreach(string[] bill in billz)
            {
                tempBill = null;
                tempBill = Instantiate(billObjPrefab, billContainer.transform);
                tempBill.GetComponent<BillObj>().PopulateBillInfo(bill[0], int.Parse(bill[1]), float.Parse(bill[2]), float.Parse(bill[3]));
                bills.Add(tempBill.GetComponent<BillObj>());
            }
        }
        else
        {
            Debug.Log("No Save Data ");
        }

        TotalBills();
    }

    public void TotalBills()
    {
        allTotalFloat = 0;
        allBillTotal = 0;

        foreach(BillObj bill in bills)
        {
            allTotalFloat += bill.bProgress;
            allBillTotal += bill.bAmount;
        }

        allTotalAmount.text = "$" + allTotalFloat.ToString("F2");
        allBillAmount.text = "$" + allBillTotal.ToString("F2");
    }

    public void PopulateBillsList()
    {
        foreach(BillObj bill in bills)
        {
            tempBill = null;
            tempBill = Instantiate(billObjPrefab, billContainer.transform);
            tempBill.GetComponent<BillObj>().PopulateBillInfo(bill.bNameObj.text, int.Parse(bill.bDueObj.ToString()), bill.bAmount, bill.bProgress);
            addEditPanel.SetActive(false);
        }
    }

    public void ClearValues()
    {
        bNameIF.text = "";
        bDueDD.value = 0;
        bTAmountIF.text = "";
        bAmountSavedIF.text = "";
    }

    public void FillEditBill(BillObj bill)
    {
        // trigger editing mode
        editCheck = true;
        addEditPanel.SetActive(true);

        // fill the input field
        bNameIF.text = bill.bNameObj.text;
        bDueDD.value = int.Parse(bill.bDueObj.text) -1;
        bTAmountIF.text = bill.totalAmountText.text;
        bAmountSavedIF.text = bill.totalSavedText.text;

        //get the bill selected
        selectedBill = bill;
    }

    public void TurnOffErrorMsg()
    {
        errorMsg.gameObject.SetActive(false);
    }

    public void SaveBillCheck()
    {
        if(bNameIF.text == "")
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = "Please enter a name for the bill";
        }
        else if (bTAmountIF.text == "")
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = "Please enter a total amount for the bill";
        }
        else if (bAmountSavedIF.text == "")
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = "Please enter an amount saved so far for the bill";
        }
        else if(float.Parse(bTAmountIF.text) < float.Parse(bAmountSavedIF.text))
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = "Please enter a saved amount that is smaller than total amount";
        }
        else
        {
            if(editCheck == false)
            {
                tempBill = null;
                tempBill = Instantiate(billObjPrefab, billContainer.transform);
                tempBill.GetComponent<BillObj>().PopulateBillInfo(bNameIF.text, bDueDD.value+1, float.Parse(bTAmountIF.text), float.Parse(bAmountSavedIF.text));

                tempBillSaveArray = new string[4];

                tempBillSaveArray[0] = bNameIF.text;
                tempBillSaveArray[1] = (bDueDD.value+1).ToString();
                tempBillSaveArray[2] = bTAmountIF.text;
                tempBillSaveArray[3] = bAmountSavedIF.text;
                billz.Add(tempBillSaveArray);

                bills.Add(tempBill.GetComponent<BillObj>());
                addEditPanel.SetActive(false);
                TotalBills();
                ES_Save.Save(billz, "UserData.src");
                
            }
            else
            {
                selectedBill.PopulateBillInfo(bNameIF.text, bDueDD.value+1, float.Parse(bTAmountIF.text), float.Parse(bAmountSavedIF.text));
                billz.Clear();

                foreach(BillObj bill in bills)
                {
                    tempBillSaveArray = new string[4];

                    tempBillSaveArray[0] = bill.bNameObj.text;
                    tempBillSaveArray[1] = bill.bDueObj.text;
                    tempBillSaveArray[2] = bill.totalAmountText.text;
                    tempBillSaveArray[3] = bill.totalSavedText.text;
                    billz.Add(tempBillSaveArray);
                }
                editCheck = false;
                addEditPanel.SetActive(false);
                TotalBills();
                ES_Save.Save(billz, "UserData.src");
                
            }
        }
    }

    public void SettingsButton()
    {
        //ES_Save.DeleteData("Userdata.src");
        /*
        foreach(GameObject bill in bills)
        {
            Debug.Log(bill.GetComponent<BillObj>().bNameObj.text + "\n" + bill.GetComponent<BillObj>().bDueObj.text + "\n" + bill.GetComponent<BillObj>().totalAmountText.text + "\n" + bill.GetComponent<BillObj>().progressText.text + "\n\n");
        }
        */
    }

    public void ChangeInteractable(bool var)
    {
        bNameIF.interactable = var;
        bTAmountIF.interactable = var;
        bAmountSavedIF.interactable = var;
        bDueDD.interactable = var;
    }

    public void DeleteBill()
    {
        //remove from bill list
        foreach(BillObj bill in bills)
        {
            if(bill.bNameObj.text == selectedBill.bNameObj.text)
            {
                bills.Remove(bill);
                Destroy(bill.gameObject);
                TotalBills();
                SaveBillList();
                return;
            }
        }
    }

    public void SaveBillList()
    {
        billz.Clear();
        foreach(BillObj bil in bills)
        {
            tempBillSaveArray = new string[4];

            tempBillSaveArray[0] = bil.bNameObj.text;
            tempBillSaveArray[1] = bil.bDueObj.text;
            tempBillSaveArray[2] = bil.totalAmountText.text;
            tempBillSaveArray[3] = bil.totalSavedText.text;
            billz.Add(tempBillSaveArray);
        }
        ES_Save.Save(billz, "UserData.src");
        
    }

    #region Sort Bill List
        public void SortList()
        {
            //Sort by Due Date
            if(bSortDD.value == 0)
            {
                bills.Sort((p1, p2) => int.Parse(p1.bDueObj.text).CompareTo(int.Parse(p2.bDueObj.text)));                
            }
            else if(bSortDD.value == 1)
            {
                bills.Sort((p1, p2) => int.Parse(p2.bDueObj.text).CompareTo(int.Parse(p1.bDueObj.text))); 
            }
            //Sort by Name
            else if(bSortDD.value == 2)
            {
                bills.Sort((p1, p2) => p1.bNameObj.text.CompareTo(p2.bNameObj.text));
            }
            else if(bSortDD.value == 3)
            {
                bills.Sort((p1, p2) => p2.bNameObj.text.CompareTo(p1.bNameObj.text));
            }
            //Sort by Progress
            else if(bSortDD.value == 4)
            {
                bills.Sort((p1, p2) => p1.bProgress.CompareTo(p2.bProgress));
            }
            else if(bSortDD.value == 5)
            {
                bills.Sort((p1, p2) => p2.bProgress.CompareTo(p1.bProgress));
            }
            //Sort by Total Bill
            else if(bSortDD.value == 6)
            {
                bills.Sort((p1, p2) => p1.bAmount.CompareTo(p2.bAmount));
            }
            else if(bSortDD.value == 7)
            {
                bills.Sort((p1, p2) => p2.bAmount.CompareTo(p1.bAmount));
            }

            for(int i = 0; i < bills.Count; i++)
            {
                bills[i].transform.SetSiblingIndex(i);
            }            
        }
    #endregion

    #region Change Progress amount with fractions buttons

        public void ChangeAmountHalf()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = (float.Parse(bTAmountIF.text) * .5f).ToString("F2");
            }
        }

        public void ChangeAmountThird()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = (float.Parse(bTAmountIF.text) * .3333f).ToString("F2");
            }
        }

        public void IncreaseByThird()
        {
            float temp = float.Parse(bAmountSavedIF.text);
            if(bAmountSavedIF.text == "")
            {
                temp = 0;
            }
            float temp2 = float.Parse(bTAmountIF.text) * .3333f;
            bAmountSavedIF.text = (temp + (temp2)).ToString("F2");
        }

        public void ChangeAmountQuarter()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = (float.Parse(bTAmountIF.text) * .25f).ToString("F2");
            }
        }

        public void IncreaseByFourth()
        {
            float temp = float.Parse(bAmountSavedIF.text);
            if(bAmountSavedIF.text == "")
            {
                temp = 0;
            }
            float temp2 = float.Parse(bTAmountIF.text) * .25f;
            bAmountSavedIF.text = (temp + (temp2)).ToString("F2");
        }

        public void ChangeAmountFifth()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = (float.Parse(bTAmountIF.text) * .2f).ToString("F2");
            }
        }

        public void IncreaseByFifth()
        {
            float temp = float.Parse(bAmountSavedIF.text);
            if(bAmountSavedIF.text == "")
            {
                temp = 0;
            }
            float temp2 = float.Parse(bTAmountIF.text) * .2f;
            bAmountSavedIF.text = (temp + (temp2)).ToString("F2");
        }

        public void ChangeAmountZero()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = 0.ToString("F2");
            }
        }

        public void ChangeAmountFull()
        {
            if(bTAmountIF.text == "")
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No Amount Entered. Please enter an amount before pressing these buttons";
                return;
            }
            else 
            {
                bAmountSavedIF.text = float.Parse(bTAmountIF.text).ToString("F2");
            }
        }

    #endregion
}
}