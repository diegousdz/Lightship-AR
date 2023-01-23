//script taken from this website 
//https://stackoverflow.com/questions/13600604/how-to-get-accurate-download-upload-speed-in-c-net
//
using UnityEngine;
using System.Net.NetworkInformation;
using TMPro;
using System;
using System.Threading;

public class BandwidthMonitor : MonoBehaviour
{

    void Update()
    {
        calculateThis();
    }

    public void calculateThis(){
        double howmuch = CheckInternetSpeed();
        print("how much" + howmuch.ToString());
    }
    public double CheckInternetSpeed()
 {
        // Create Object Of WebClient
        System.Net.WebClient wc = new System.Net.WebClient();

        //DateTime Variable To Store Download Start Time.
        DateTime dt1 = DateTime.UtcNow;

        //Number Of Bytes Downloaded Are Stored In ‘data’
        byte[] data = wc.DownloadData("http://google.com");

        //DateTime Variable To Store Download End Time.
        DateTime dt2 = DateTime.UtcNow;

        //To Calculate Speed in Kb Divide Value Of data by 1024 And Then by End Time Subtract Start Time To Know Download Per Second.
        return Math.Round((data.Length / 1024) / (dt2 - dt1).TotalSeconds, 2);            
    }

}