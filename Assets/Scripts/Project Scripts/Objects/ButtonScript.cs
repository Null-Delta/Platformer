using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonScript : EventTrigger
{
    Sprite button, button_pressed;
    PlayerControl pc;

    void Start()
    {
        pc = Camera.main.GetComponent<PlayerControl>();
        button = Resources.Load<Sprite>("Textures/button/" + gameObject.name);
        button_pressed = Resources.Load<Sprite>("Textures/button/"+ gameObject.name + "_pressed");
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public override void OnPointerDown(PointerEventData data)
    {
        gameObject.GetComponent<Image>().sprite = button_pressed;
        pc.Move(Int32.Parse(gameObject.name.Remove(0,7)));
    }

    public override void OnPointerUp(PointerEventData data)
    {
        gameObject.GetComponent<Image>().sprite = button;
    }
}
