using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DebugScreen : MonoBehaviour {
    GUIStyle style = new GUIStyle();
    int accumulator = 0;
    int counter = 0;
    float timer = 0f;
 
    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 200;

        style.normal.textColor = Color.white;
        style.fontSize = 32;
        style.fontStyle = FontStyle.Bold;
    }
 
    void OnGUI() {
        GUI.Label(new Rect(50, 10, 100, 34), "FPS: " + counter, style);
    }
 
    void Update() {
        accumulator++;
        timer += Time.deltaTime;
 
        if (timer >= 1f) {
            timer = 0;
            counter = accumulator;
            accumulator=0;
        }
    }
}