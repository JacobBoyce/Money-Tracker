using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    //selected alpha 73
    public List<GameObject> menus = new List<GameObject>();

    void Start()
    {
        ChangeMenu("MainMenu");
    }
    
    public void ChangeMenu(GameObject menu)
    {
        for(int i = 0; i < menus.Count; i++)
        {
            if(menu == menus[i])
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    public void ChangeMenu(string menu)
    {
        for(int i = 0; i < menus.Count; i++)
        {
            if(menu == menus[i].name)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
}
