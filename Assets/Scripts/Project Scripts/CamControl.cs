using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CamControl : MonoBehaviour
{
    public GameObject targetObj;
    
    [SerializeField] float minSize = 0.5f, maxSize = 30;
    [SerializeField] Vector2 saveZoneEdge1 = new Vector2(-10,-10), saveZoneEdge2 = new Vector2(276, 276);
    [SerializeField] float startCamSize = 5;   
    [SerializeField] Vector2 startCamPosition = new Vector2 (15,15);
    [SerializeField] float smoothFollowingСursor = 16;
    [SerializeField] float smoothResizing = 16;
    [SerializeField] float forceZoom = 16;
    [SerializeField] float targetApproachSpeed = 16;
    [SerializeField] float cameraInertiaForce = 65;
    private Vector2 pointerPos, posForZoom1 = Vector2.zero, posForZoom2 = Vector2.zero, futurePos;

    //[SerializeField] bool goTarget = false;  Нигде не использовалось
    [SerializeField] bool followingTarget = false;

    float x, y, size, futureSize = 0, difSize = 1, inertiaCam = 0;
    RuntimePlatform platform = Application.platform;
    
    InputHandler ih;


    void Start()
    {
        ih = Camera.main.GetComponent<InputHandler>();
        
        Camera.main.orthographicSize = startCamSize;
        futureSize = startCamSize;
        Camera.main.transform.position = new Vector3(startCamPosition.x, startCamPosition.y, -10);
        
        // switch(platform)
        // {
        //     case RuntimePlatform.WindowsEditor:
        //     case RuntimePlatform.WindowsPlayer:
        //     case RuntimePlatform.OSXEditor:
        //     case RuntimePlatform.OSXPlayer:
                
        //     break;

        //     case RuntimePlatform.Android:
        //     case RuntimePlatform.IPhonePlayer:
        //         smoothFollowingСursor = 2;
        //         smoothResizing = 4;
        //         cameraInertiaForce = 50;
        //     break;
        // }
    }

    

    

    

    


    void goToTargetPos(Vector2 targetPos)
    {
        Vector2 heading = targetPos - (Vector2)Camera.main.transform.position;

        Vector2 heading2 = (Vector2)Camera.main.transform.position - targetPos;

        heading2/=heading2.magnitude;

        if (heading.sqrMagnitude < 0.01f * 0.01f)
        {
            //goTarget = false; Нигде не использовалось
            Camera.main.transform.position = targetPos;
        }
        else
        {
            Camera.main.transform.position += (Vector3)((heading - heading2)* Time.deltaTime * targetApproachSpeed/8);       
        }    
    }
    
    void trackingTargetPos()
    {
        targetObj = Camera.main.GetComponent<PlayerControl>().CurrentPlayer.gameObject;

        Vector2 targetPos = targetObj.transform.position;

        Vector2 heading = targetPos - (Vector2)Camera.main.transform.position;

        Vector2 heading2 = (Vector2)Camera.main.transform.position - targetPos;

        if (heading.sqrMagnitude < 0.01f * 0.01f)
        {
            Camera.main.transform.position = targetPos;
            Camera.main.transform.position += new Vector3(0,0,-10);
        }
        else
        {
            Camera.main.transform.position += (Vector3)((heading - heading2/4) * Time.deltaTime * targetApproachSpeed/8);    
        }  
    }

    void Update()
    {   
        if(!followingTarget)
        {
            if(ih.trakingButtonDown())
                followingTarget = true;
        }
        else
        {
            if(ih.trakingButtonDown())
                followingTarget = false;
        }
            

        if (followingTarget)
        {
            inertiaCam = 0;
            trackingTargetPos();
        }
        else
        {
            //Сохраняяем координаты мышки и камеры
            if (ih.PointerDown())
                pointerPos = ih.GetPointerPos();

            
            if (ih.Pointer())
            {
                Vector2 pointerPosDinamic = ih.GetPointerPos();
                futurePos = pointerPos - pointerPosDinamic;

                Vector2 differencePos = futurePos/smoothFollowingСursor*(Time.deltaTime*200);
                Vector2 interpolationPos = Camera.main.transform.position + (Vector3)differencePos;

                //Обработка saveZone
                if(interpolationPos.x < saveZoneEdge1.x || interpolationPos.x > saveZoneEdge2.x)
                    differencePos = new Vector2(0, differencePos.y);
                if(interpolationPos.y < saveZoneEdge1.y || interpolationPos.y > saveZoneEdge2.y)
                    differencePos = new Vector2(differencePos.x, 0);

                Camera.main.transform.position += (Vector3)differencePos;

                inertiaCam = 1;
            }
            else
            {
                // Если скорость в пт/кадр больше 0.2, уменьшать её.
                // 
                if((Math.Abs(futurePos.x)+Math.Abs(futurePos.y))*inertiaCam > 0.2f)
                {
                    Vector2 differencePos = futurePos/smoothFollowingСursor*inertiaCam*(Time.deltaTime*200);
                    Vector2 interpolationPos = Camera.main.transform.position + (Vector3)differencePos;

                    //Обработка saveZone
                    if(interpolationPos.x < saveZoneEdge1.x || interpolationPos.x > saveZoneEdge2.x)
                        differencePos = new Vector2(0, differencePos.y);
                    if(interpolationPos.y < saveZoneEdge1.y || interpolationPos.y > saveZoneEdge2.y)
                        differencePos = new Vector2(differencePos.x, 0);

                    Camera.main.transform.position += (Vector3)differencePos;
                    inertiaCam/=(1+(1f/cameraInertiaForce));
                }
                else inertiaCam = 0;
            }
            
        }

        // if (goTarget)
        // {
        //     goToTargetPos(targetPos);
        //     if (ih.PointerDown())
        //         mousePos = ih.GetPointerPos();
        //     inertiaCam = 0;   
        // }   
        // else
        // {
            //followingTarget = false;
            

        //Расчёт будущего размера камеры
        float deltaY = ih.GetDeltaY();
        if (deltaY != 0)
        {
            inertiaCam = 0;
            float k = futureSize*(forceZoom/200);
            if ((futureSize - (deltaY * k) >= minSize) && (futureSize - (deltaY * k) <= maxSize))
            {
                //k = futureSize/(150/forceZoom*(Time.deltaTime*150));
                futureSize += -(deltaY) * k;
            }
        }

        //Плавное изменение размера камеры
        size = Camera.main.orthographicSize;
        if (size != futureSize)
        {
            posForZoom1 = ih.GetPosForZoom();
            
            difSize = Math.Abs(size - futureSize);
            if (difSize < 0.025f) difSize = 0.025f;
            if (size > futureSize) difSize = -difSize;
            
            if (size != futureSize)
            {                     
                Camera.main.orthographicSize += 100/smoothResizing * difSize * Time.deltaTime;
                if (Math.Abs(Camera.main.orthographicSize - futureSize) < 0.01f)
                    Camera.main.orthographicSize = futureSize;
            }

            if(!followingTarget)
            {
                posForZoom2 = ih.GetPosForZoom();
                

                Vector2 differencePos = posForZoom1 - posForZoom2;
                Vector2 interpolationPos = Camera.main.transform.position + (Vector3)differencePos;
                //Обработка saveZone
                if(interpolationPos.x < saveZoneEdge1.x || interpolationPos.x > saveZoneEdge2.x)
                    differencePos = new Vector2(0, differencePos.y);
                if(interpolationPos.y < saveZoneEdge1.y || interpolationPos.y > saveZoneEdge2.y)
                    differencePos = new Vector2(differencePos.x, 0);

                //Движение камеры к курсору при приближении и обратно
                Camera.main.transform.position += (Vector3)differencePos;
            }
        }
    }
}
