using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace QuantumTek.EncryptedSave
{

public class Seperator : MonoBehaviour
{
    public TMP_InputField inF, outF;
    public GameObject sepaPrefab, sepaContainer, tempSepa, deleteButton, errorMSGObj;
    public List<GameObject> sepaList = new List<GameObject>();
    public List<string[]> sepaSaveList = new List<string[]>();
    public string[] infoutf;
    public string[] tempSepaSaveArray;
    private string temp, errorMsg;
    public bool errorOn;

    public float totalAdded = 0, inAmount = 0;
    
    void Start()
    {
        tempSepaSaveArray = new string[2];
        infoutf = new string[2];
        sepaList.Clear();
        sepaSaveList.Clear();
        errorOn = false;

        if(ES_Save.Exists("UserDataSepa.src"))
        {
            sepaSaveList = ES_Save.Load<List<string[]>>("UserdataSepa.src");
            foreach(string[] sepa in sepaSaveList)
            {
                tempSepa = null;
                tempSepa = Instantiate(sepaPrefab, sepaContainer.transform);
                Debug.Log(sepa[0] + "  " + sepa[1]);
                tempSepa.transform.Find("NameTag").gameObject.GetComponent<TMP_InputField>().text = sepa[0];
                tempSepa.transform.Find("AmountTag").gameObject.GetComponent<TMP_InputField>().text = sepa[1];
                sepaList.Add(tempSepa);
            }
        }
        else
        {
            Debug.Log("No Save Data UserdataSepa");
        }

        if(ES_Save.Exists("UserDataSepaBigVal.src"))
        {
            infoutf = ES_Save.Load<string[]>("UserDataSepaBigVal.src");
            //load inf and outf
            inF.text = infoutf[0];
            outF.text = infoutf[1];
        }
        else
        {
            Debug.Log("No Save Data UserDataSepaBigVal");
        }
    }

    public void UpdateAfter()
    {
        totalAdded = 0;
        inAmount = float.Parse(inF.text);
        sepaSaveList.Clear();
        //deactivate erromsg obj
        errorMSGObj.SetActive(false);
        errorOn = false;
        

        //ERROR CHECK
        foreach(GameObject sepa in sepaList)
        {
            if(sepa.transform.Find("AmountTag").gameObject.GetComponent<TMP_InputField>().text.Equals("") || sepa.transform.Find("NameTag").gameObject.GetComponent<TMP_InputField>().text.Equals(""))
            {
                //seterror message active the set message
                errorMSGObj.SetActive(true);
                errorMsg = "Please enter all titles and amounts";
                errorMSGObj.GetComponent<TMP_Text>().text = errorMsg;
                errorOn = true;
            }
        }

        if(errorOn == false)
        {
            foreach(GameObject sepa in sepaList)
            {
                tempSepaSaveArray = new string[2];
                tempSepaSaveArray[0] = sepa.transform.Find("NameTag").gameObject.GetComponent<TMP_InputField>().text;
                tempSepaSaveArray[1] = sepa.transform.Find("AmountTag").gameObject.GetComponent<TMP_InputField>().text;

                sepaSaveList.Add(tempSepaSaveArray);
                
                totalAdded += float.Parse(sepa.transform.Find("AmountTag").gameObject.GetComponent<TMP_InputField>().text);
            }

            ES_Save.Save(sepaSaveList, "UserdataSepa.src");

            totalAdded =  inAmount - totalAdded;
            totalAdded = (float)Math.Round(totalAdded, 3);
            outF.text = totalAdded.ToString();

            infoutf[0] = inF.text;
            infoutf[1] = outF.text;
            ES_Save.Save(infoutf, "UserDataSepaBigVal.src");
        }
    }

    public void AddSepa()
    {
        tempSepa = null;
        tempSepa = Instantiate(sepaPrefab, sepaContainer.transform);
        sepaList.Add(tempSepa);
    }

    public void DeleteSepa()
    {
        foreach(string[] sepa in sepaSaveList)
        {
            if(sepa[0] == temp)
            {
                tempSepaSaveArray = sepa;
            }
        }
        sepaSaveList.Remove(tempSepaSaveArray);
        sepaList.Remove(tempSepa);
        Destroy(tempSepa);
        deleteButton.SetActive(false);
    }

    public void SelectAndDeselect(GameObject sepa)
    {
        foreach(GameObject sepaObj in sepaList)
        {
            if(sepaObj == sepa)
            {
                temp = sepa.transform.Find("NameTag").gameObject.GetComponent<TMP_InputField>().text;
                sepaObj.GetComponent<SepaOBJ>().selectedPanel.SetActive(true);
                tempSepa = sepaObj;
                deleteButton.SetActive(true);
            }
            else
            {
                sepaObj.GetComponent<SepaOBJ>().selectedPanel.SetActive(false);
            }
        }
    }
    //when delete button pressed
    //activate the select button on each sepa obj
    // when sepa is clicked ask for an are you sure?
    //then remove from list
}
}