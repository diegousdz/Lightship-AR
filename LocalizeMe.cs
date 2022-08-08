using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Android;

public class LocalizeMe : MonoBehaviour
{
    public TMP_Text Lat;
    public TMP_Text Long;
    public temporalMetadata tempMeta;
    [HideInInspector]
    public float freaquencyRate = 15f;

    IEnumerator AskForPermision()
    {
        //start localization with
        UnityEngine.Input.location.Start(1f, 1f);
        yield return new WaitForSeconds(1);
        StartCoroutine(updateGPS());
    }

    public void CaptureOnetime()
    {
        StartCoroutine(AskForPermision());
    }

    public void StartLocation()
    {
        
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
        }
        else
        {
            // Access granted and location value could be retrieved
       //     StartCoroutine(locationProcess());
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            Lat.text = Input.location.lastData.latitude.ToString();
            Long.text = Input.location.lastData.longitude.ToString();
        }
    }

    IEnumerator updateGPS()
    {

        float UPDATE_TIME = freaquencyRate; //Every  3 seconds
        WaitForSeconds updateTime = new WaitForSeconds(UPDATE_TIME);
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
        }
        else
        {
            while (true)
            {
                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                Lat.text = Input.location.lastData.latitude.ToString();
                Long.text = Input.location.lastData.longitude.ToString();

                tempMeta.latTemp = Input.location.lastData.latitude;
                tempMeta.longTemp = Input.location.lastData.longitude;
                yield return updateTime;
            }
        }
    }

    public void StopLocation()
    {
        Input.location.Stop();
        StopCoroutine(updateGPS());
        Lat.text = "";
        Long.text = "";
    }
}
