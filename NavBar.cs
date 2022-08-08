using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBar : MonoBehaviour
{

    public GameObject B1On;
    public GameObject B1Off;
    public GameObject B2On;
    public GameObject B2Off;
    public GameObject B3On;
    public GameObject B3Off;

    public GameObject MapScreen;
    public GameObject OrdersScreen;
    public GameObject ProfileScreen;

    public GameObject SafetyWarning;

    public GameObject Cam1;
    public GameObject[] MapObj;
    public GameObject LocationManager;

    private void Start()
    {
        SafetyWarning.SetActive(true);
        mapSelected();
    }

    public void mapSelected()
    {
        B1Off.SetActive(false);
        B1On.SetActive(true);

        B2Off.SetActive(true);
        B2On.SetActive(false);
        
        B3Off.SetActive(true);
        B3On.SetActive(false);

        MapScreen.SetActive(true);
        OrdersScreen.SetActive(false);
        ProfileScreen.SetActive(false);

        Cam1.SetActive(false);

        for (int i = 0; i < MapObj.Length; i++)
        {
            MapObj[i].SetActive(true);
        }
    }

    public void ordersSelected()
    {
        B1Off.SetActive(true);
        B1On.SetActive(false);

        B2Off.SetActive(false);
        B2On.SetActive(true);

        B3Off.SetActive(true);
        B3On.SetActive(false);
        
        MapScreen.SetActive(false);
        OrdersScreen.SetActive(true);
        ProfileScreen.SetActive(false);

        Cam1.SetActive(true);

        if(LocationManager.activeSelf)
        {
            for (int i = 0; i < MapObj.Length; i++)
            {
            MapObj[i].SetActive(false);
            }
        }
    }

    public void profileSelected()
    {
        B1Off.SetActive(true);
        B1On.SetActive(false);

        B2Off.SetActive(true);
        B2On.SetActive(false);

        B3Off.SetActive(false);
        B3On.SetActive(true);

        MapScreen.SetActive(false);
        OrdersScreen.SetActive(false);
        ProfileScreen.SetActive(true);

        Cam1.SetActive(true);

        if(LocationManager.activeSelf)
        {
            for (int i = 0; i < MapObj.Length; i++)
            {
            MapObj[i].SetActive(false);
            }
        }    
    }
}
