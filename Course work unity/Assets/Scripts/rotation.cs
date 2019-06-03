﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rotation : MonoBehaviour
{
    //private float rotationTime = 1f;
    public static bool movingMode;        /////// 0 - horizontal, 1 - vertical ///////
    private Quaternion qAngle;
    private Vector3 playerCurRot;
    private Vector3 playerRotPos;
    private bool stabilized;
    public bool inRotation;
    private Vector3 velocity;
   // private float playerCurRotEulerY;

   // private Rigidbody playerRB;

    //private float lerpTimer;

    private  float rotStabilizeTime;

    void Start()
    {
        qAngle  = new Quaternion();
        //playerCurRotEulerY = 0;
       // moving.PlayerRB = GetComponent<Rigidbody>();
       // Stabilized = true;
        playerCurRot = new Vector3(0,90,0);
        playerCurRot.z = 0;
        qAngle.z = 0;
        rotStabilizeTime = 0;
        playerRotPos = transform.position;
        inRotation = false;
        rotStabilize(0f);
        movingMode = false;
        stabilized = true;
        //lerpTimer = 0f;
        //qAngle = Quaternion.Euler(transform.rotation.eulerAngles);
        playerRotPos.x = Mathf.Round(playerRotPos.x);
        playerRotPos.z = Mathf.Round(playerRotPos.z);
        //StartCoroutine("rotTimer");
        StopAllCoroutines();
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
                                      RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {       
        playerRotPos = transform.position;

        if(movingMode)
        {
            playerRotPos.x = Mathf.Round(playerRotPos.x);
        }
        else
        {
            playerRotPos.z = Mathf.Round(playerRotPos.z);
        }
      //  Debug.Log(rotStabilizeTime);
       // Debug.Log(playerCurRot);
      //  Debug.Log(transform.rotation);
        Debug.Log(velocity);
        playerCurRot.x = transform.rotation.eulerAngles.x;

       // qAngle.y = Mathf.LerpUnclamped(transform.rotation.y, playerCurRot.y, rotStabilizeTime);
        //qAngle.z = Mathf.LerpUnclamped(transform.rotation.z, 0f, rotStabilizeTime);
       // qAngle.w = transform.rotation.w;
       // qAngle.x = transform.rotation.x;
       // qAngle.y = Mathf.LerpUnclamped(transform.rotation.y, playerCurRot.y, rotStabilizeTime);
       // qAngle.z = Mathf.LerpUnclamped(transform.rotation.z, 0f, rotStabilizeTime);
        //qAngle.w = transform.rotation.w;
              //  qAngle.z = Mathf.LerpUnclamped(transform.rotation.z, 0f, rotStabilizeTime);
     //   qAngle.w = transform.rotation.w;
    }
    void FixedUpdate()
    {
       // qAngle.x = transform.rotation.x;
       // qAngle.y = Mathf.LerpUnclamped(transform.rotation.y, playerCurRot.y, rotStabilizeTime);
       // qAngle.w = transform.rotation.w;
        if(!inRotation)
        {
           // playerRB.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(playerCurRot) ,  rotStabilizeTime));
            if(!stabilized)
            {
                Moving.PlayerRB.MoveRotation(Quaternion.Normalize(qAngle));
            }
            else
            {

            }
           // playerRB.MoveRotation(new Quaternion(transform.rotation.x, ));
            Moving.PlayerRB.MovePosition(Vector3.Lerp(transform.position, playerRotPos, rotStabilizeTime+0.01f));
        }
    }
    public void rotStabilize(float angle) 
    {
        playerCurRot.y += angle;
        Moving.PlayerRB.constraints = RigidbodyConstraints.None;
        StartCoroutine("rotTimer");
    }
    public void setRotJoint(byte rotMode)
    {
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationZ
                                    | RigidbodyConstraints.FreezeRotationX;
        //playerRB.constraints = RigidbodyConstraints.None;
        gameObject.AddComponent(typeof(ConfigurableJoint));
        ConfigurableJoint CJ = GetComponent<ConfigurableJoint>();
        SoftJointLimit CJlimit = new SoftJointLimit();
        CJlimit.limit = 0.32f;
        CJlimit.bounciness = 0;
        CJlimit.contactDistance = 0;

        CJ.xMotion = ConfigurableJointMotion.Limited;
        CJ.zMotion = ConfigurableJointMotion.Limited;
        CJ.anchor = new Vector3(-0.32f * Mathf.Pow(-1, rotMode), 0, 0);
        CJ.linearLimit = CJlimit;
    }
    public void resetRotJoint()
    {
        //playerRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        Destroy(GetComponent<ConfigurableJoint>());
    }
    
    public Vector3 getCurRot()
    {
        return playerCurRot;
    }
    public void deltaCurRotX(float delta)
    {
        if(delta == 0)
        {
            playerCurRot.x = 0;
            return;
        }
        //playerCurRot.x = transform.rotation.;
        playerCurRot.x = delta;
        if(playerCurRot.x >= 360 || playerCurRot.x <= -360)
        {
            playerCurRot.x = 0;
        }
    }

    IEnumerator rotTimer()
    {
        StartCoroutine("getX");
       // qAngle = Quaternion.Euler(playerCurRot);
        velocity = Moving.PlayerRB.velocity;
        stabilized = false;
        Moving.PlayerRB.Sleep();
        yield return null;
        Moving.PlayerRB.WakeUp();
        Moving.PlayerRB.velocity = velocity;
      //  if(playerRB.velocity.y != 0)
      //  {
      //      rotStabilizeTime = 0.5f;
      //  }
       // yield return new WaitUntil(() => playerRB.velocity.y == 0);
        /*while(transform.rotation.eulerAngles.y != playerCurRotEulerY && playerRB.velocity.y != 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * Mathf.Sign(transform.rotation.eulerAngles.y  - playerCurRotEulerY));
            yield return null;
        }*/
       // yield return new WaitUntil(() => Mathf.Abs(playerRB.velocity.y) <= 0.5f);
     //   if(!movingMode)
      //  {
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(2f);
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationY;
                                   // | RigidbodyConstraints.FreezeRotationZ;
                                  // RigidbodyConstraints.FreezePositionZ;
       // }
       /*else
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
                                   RigidbodyConstraints.FreezeRotationY;
                                 //  RigidbodyConstraints.FreezePositionX;
        }*/
        stabilized = true;
        StopCoroutine("getX");
        rotStabilizeTime = 0;
        yield return new WaitUntil(() => transform.rotation.eulerAngles.z <= 0.0001f && transform.rotation.eulerAngles.z >= -0.0001f);
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        StopCoroutine("rotTimer");
        //moving.PlayerRB.constraints |= RigidbodyConstraints.FreezePositionZ;
    }
    IEnumerator getX()
    {
        while(true)
        {
           // playerCurRot.x = transform.rotation.eulerAngles.x;
            rotStabilizeTime += Time.deltaTime/10;
            playerCurRot.x = transform.rotation.eulerAngles.x;
            qAngle = Quaternion.Euler(playerCurRot);
           // playerCurRot.x = transform.rotation.eulerAngles.x;
           // qAngle.w = transform.rotation.w;
            yield return new WaitForEndOfFrame();
        }
     //   yield return null;
    }
}
