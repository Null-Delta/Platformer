using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CamControl : MonoBehaviour
{
    public GameObject targetObj;
    
    public float minSize = 1, maxSize = 50;
    public float smoothFollowingСursor = 16;
    public float smoothResizing = 16;
    public float forceApproach = 16;
    public float targetApproachSpeed = 16;
    public float cameraInertiaForce = 65, cameraInertiaDuration = 0.5f;
    private Vector2 mousePos, mousePosDinamic, posForZoom1 = Vector2.zero, posForZoom2 = Vector2.zero, futureCoord = Vector2.zero;
    public Vector3 targetPos;
    public bool goTarget = false, followingTarget = true, cameraMove = true, touchDown = false;

    float x, y, k = 1, size, futureSize = 0, k2 = 1, zCam = -10, delay = 0, futureX, futureY, time = 0, inertiaCam = 0;
    RuntimePlatform platform = Application.platform;
    
    void Start()
    {
        Camera.main.orthographicSize = 17;
        futureSize = Camera.main.orthographicSize;
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {

        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            smoothFollowingСursor = 8;
            smoothResizing = 8;
            cameraInertiaForce = 50;
            cameraInertiaDuration = 0.3f;
        }
    }

    bool pointer()
    {
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButton(1))
                return true;
            
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount == 1)
                    return true;
        }
        
        return false;
    }

    bool pointerDown()
    {
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(1))
                return true;
            
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount != 1) touchDown = true;

            if (Input.touchCount == 1 && touchDown)
                {
                    touchDown = false;
                    return true;
                }

                    
        }

        return false;
    }

    float inputDeltaY()
    {   
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            return Input.mouseScrollDelta.y; 
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            
            if (Input.touchCount == 2)
            {
                cameraMove = false;
                Touch touch0  = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 posTouch0 = touch0.position - touch0.deltaPosition;
                Vector2 posTouch1 = touch1.position - touch1.deltaPosition;
                
                float disTouch = (posTouch0 - posTouch1).magnitude;
                float curDisTouch = (touch0.position - touch1.position).magnitude;

                float dif = curDisTouch - disTouch;
                
                return dif*0.01f;
            }
            return 0;  
        }

        return 0;
    }

    Vector2 inputPosForZoom()
    {
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            
            if (Input.touchCount == 2)
            {
                cameraMove = false;
                Touch touch0  = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 posTouch0 = touch0.position - touch0.deltaPosition;
                Vector2 posTouch1 = touch1.position - touch1.deltaPosition;
                return Camera.main.ScreenToWorldPoint((posTouch0+posTouch1)/2);
            }
            else return Vector2.zero;
        }
        return Vector2.zero; 
    }

    Vector2 inputPos()
    {
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        return Vector2.zero; 
    }


    void goToTargetPos(Vector3 targetPos)
    {
        if (followingTarget) targetPos = targetObj.transform.position;

        targetPos -= new Vector3(0,0,targetObj.transform.position.z - zCam);

        Vector3 heading = targetPos - Camera.main.transform.position;

        Vector3 heading2 = Camera.main.transform.position - targetPos;

        heading2/=heading2.magnitude;
        if (heading.sqrMagnitude < 0.01f * 0.01f)
        {
            if(!followingTarget) goTarget = false;
            Camera.main.transform.position = targetPos;
        }
        else
        {
            if (followingTarget) Camera.main.transform.position += (heading - heading2/4)*Time.deltaTime * targetApproachSpeed/8;
            else Camera.main.transform.position += (heading - heading2)*Time.deltaTime * targetApproachSpeed/8;       
        }    
    }
    
    void trackingMode()
    {
        if (platform == RuntimePlatform.WindowsEditor)
        {
            
            if(Input.GetKeyDown(KeyCode.C))
            {
                if (Math.Abs(time- Time.time) < 0.3f) followingTarget = true;
                else followingTarget = false;
                goTarget = true;
                targetPos = targetObj.transform.position;
                time = Time.time;
            }

            
        }
        else if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            
        }

        
    }

    void Update()
    {   
        float deltaY = inputDeltaY();

        
        trackingMode();
        
        
        if (goTarget)
        {
            goToTargetPos(targetPos);
            if (pointerDown())
                mousePos = inputPos();
            inertiaCam = 0;   
        }   
        else
        {
            //followingTarget = false;
            //Сохраняяем координаты мышки и камеры
            if (pointerDown())
                mousePos = inputPos();

            //Перемещаем камеру
            if (pointer())
            {
                mousePosDinamic = inputPos();
                futureX = mousePos.x - mousePosDinamic.x;
                futureY = mousePos.y - mousePosDinamic.y;
                inertiaCam = cameraInertiaDuration;
            }
            
            if (inertiaCam > 0.02f || futureX/smoothFollowingСursor*(inertiaCam/cameraInertiaDuration)> 0.035f)
            {
                Camera.main.transform.position += new Vector3 (futureX/smoothFollowingСursor*(inertiaCam/cameraInertiaDuration),futureY/smoothFollowingСursor*(inertiaCam/cameraInertiaDuration), 0);
                inertiaCam/=(1+(1f/cameraInertiaForce));
            }
        }
        //Расчёт будущего размера камеры
        if (deltaY != 0)
        {
            if ((futureSize - (deltaY) * k >= minSize) && (futureSize - (deltaY) * k <= maxSize))
            {
                k = futureSize / (200/forceApproach);
                futureSize += -(deltaY) * k;
                
            }
        }

        //Плавное изменение размера камеры
        size = Camera.main.orthographicSize;
        if (size != futureSize)
        {
            posForZoom1 = inputPosForZoom();
            k2 = Math.Abs(size - futureSize);
            if (k2 < 0.025f) k2 = 0.025f;
            if (size > futureSize) k2 = -k2;
            
            if (size != futureSize)
            {                     
                Camera.main.orthographicSize += 100/smoothResizing * k2 * Time.deltaTime;
                if (Math.Abs(Camera.main.orthographicSize - futureSize) < 0.01f)
                    Camera.main.orthographicSize = futureSize;
            }

            if(!goTarget)
            {
                posForZoom2 = inputPosForZoom();
            //Движение камеры к курсору при приближении и обратно
            transform.position += (Vector3)(posForZoom1 - posForZoom2);
            }
        }
    }
}
