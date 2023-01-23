//creedits to chatGTP to with whom I created this script
//22 January 22
using UnityEngine;
using System.Net.NetworkInformation;
using TMPro;

public class InternetSpeed : MonoBehaviour
{
    public TMP_Text dataUsageText;
    private long bytesReceived;
    private long bytesSent;
    private float updateInterval = 1.0f; // Update data usage every second
    private float nextUpdateTime;

  void Start()
{
    // Get the initial data usage values
    bytesReceived = GetBytesReceived();
    bytesSent = GetBytesSent();
    nextUpdateTime = Time.time + updateInterval;
    Debug.Log("Initial bytes received: " + bytesReceived);
    Debug.Log("Initial bytes sent: " + bytesSent);
}

    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            // Get the current data usage values
            long currentBytesReceived = GetBytesReceived();
            long currentBytesSent = GetBytesSent();

            // Calculate the data usage since the last update
            long deltaBytesReceived = currentBytesReceived - bytesReceived;
            long deltaBytesSent = currentBytesSent - bytesSent;

            // Update the data usage text
            dataUsageText.text = string.Format("Bandwidth: {0} KB/s Received, {1} KB/s Sent",
                                            deltaBytesReceived / 1024, deltaBytesSent / 1024);

            // Update the previous data usage values
            bytesReceived = currentBytesReceived;
            bytesSent = currentBytesSent;
            nextUpdateTime = Time.time + updateInterval;
            Debug.Log("Delta bytes received: " + deltaBytesReceived);
            Debug.Log("Delta bytes sent: " + deltaBytesSent);
        }
    }

    long GetBytesReceived()
    {
        long bytesReceived = 0;
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            bytesReceived += ni.GetIPv4Statistics().BytesReceived;
            Debug.Log("Bytes received from " + ni.Name + ": " + ni.GetIPv4Statistics().BytesReceived);
        }
        return bytesReceived;
    }

    long GetBytesSent()
    {
        long bytesSent = 0;
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            bytesSent += ni.GetIPv4Statistics().BytesSent;
            Debug.Log("Bytes sent from " + ni.Name + ": " + ni.GetIPv4Statistics().BytesSent);
        }
        return bytesSent;
    }

}
