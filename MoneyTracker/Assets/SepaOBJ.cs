using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QuantumTek.EncryptedSave
{
public class SepaOBJ : MonoBehaviour
{
    public GameObject selectedPanel;
    public void SelectedOrDeselcted()
    {
        GameObject.Find("SeperatorMenu").GetComponent<Seperator>().SelectAndDeselect(this.gameObject);
    }
}
}
