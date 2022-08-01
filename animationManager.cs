using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    public Animator anim;
    public GameObject anchorAvatar;
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 200.0f;
    public float x;
    public float y;

    public void Update()
    {
        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        var tempRot = new Vector3(0,x * Time.deltaTime * rotationSpeed, 0);
        anchorAvatar.transform.Rotate(tempRot);   
        var tempTrans = new Vector3(0,0,y * Time.deltaTime * movementSpeed);
        anchorAvatar.transform.Translate(tempTrans);
        if(y > 0.2){
            anim.Play("walking1");
        } 
        if(y < 0.1){
            if(y>=0){
                anim.Play("idle");
            }
        }
        if(y >= 0){
            if(y<=0.1){
                anim.Play("idle");
            }
        }

        if(y <= -0.1){
            anim.Play("WalkingBackward");
        }  
    }
}











