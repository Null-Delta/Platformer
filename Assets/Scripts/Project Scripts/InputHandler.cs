using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    RuntimePlatform platform = Application.platform;

    bool touchDown = false;
    public bool Pointer()
    {
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                if (Input.GetMouseButton(1))
                    return true;
            break;

            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount == 1)
                    return true;
            break;
        }
        return false;
    }

    public bool PointerDown()
    {
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                if (Input.GetMouseButtonDown(1))
                return true;
            break;

            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount != 1) touchDown = true;
                if (Input.touchCount == 1 && touchDown)
                    {
                        touchDown = false;
                        return true;
                    }
            break;
        }
        return false;
    }

    
    public bool trakingButtonDown()
    {
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                if (Input.GetKeyDown(KeyCode.T))
                    return true;
            break;

            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                
            break;
        }
        return false;
    }

    public float GetDeltaY()
    {   
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return Input.mouseScrollDelta.y; 

            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount >= 2)
                {
                    Touch touch0  = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);

                    Vector2 posTouch0 = touch0.position - touch0.deltaPosition;
                    Vector2 posTouch1 = touch1.position - touch1.deltaPosition;
                    
                    float disTouch = (posTouch0 - posTouch1).magnitude;
                    float curDisTouch = (touch0.position - touch1.position).magnitude;

                    float dif = curDisTouch - disTouch;
                    
                    return dif*0.01f;
                }
                else return 0;
        }
        return 0;
    }
    public Vector2 GetPosForZoom()
    {
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount >= 2)
                    {
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

    public Vector2 GetPointerPos()
    {
        switch(platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return Camera.main.ScreenToWorldPoint(Input.mousePosition); 

            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        return Vector2.zero; 
    }
}
