using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetConectionManager : MonoBehaviour
{
    public NotificationBar notificationManager;
     string m_ReachabilityText;

    public void checkForInternetConection()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            //set the value of the message
            notificationManager.NotificationBody = "Error. Check internet connection!";
            //show notification
            notificationManager.startSequenceNotification();
        }
    }

    void Update()
    {
        
        //Output the network reachability to the console window
     //   Debug.Log("Internet : " + m_ReachabilityText);
        //Check if the device cannot reach the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            m_ReachabilityText = "Not Reachable.";
            Debug.Log("Internet : " + m_ReachabilityText);
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            m_ReachabilityText = "Reachable via carrier data network.";
            Debug.Log("Internet : " + m_ReachabilityText);
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            m_ReachabilityText = "Reachable via Local Area Network.";
            Debug.Log("Internet : " + m_ReachabilityText);
        }
    }

}
