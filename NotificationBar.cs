using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationBar : MonoBehaviour
{
    public Animator anim;
    public TMP_Text NotificationText;

    [HideInInspector]
    public string NotificationBody;

    public void startSequenceNotification()
    {
        StartCoroutine(NotificationRoutine());
    }

    public void dataParse()
    {
        NotificationText.text = NotificationBody;
    }

    public void OpenNotificationBar()
    {
        anim.Play("SlideIn");
    }

     public void CloseNotificationBar()
    {
        anim.Play("SlideOut");
    }

    IEnumerator NotificationRoutine()
    {
        dataParse();
        yield return new WaitForSeconds(0.1f);
        OpenNotificationBar();
        yield return new WaitForSeconds(2);
        CloseNotificationBar();
        yield return new WaitForSeconds(1);
        NotificationBody = "";
        NotificationText.text = "";
    }

}
