

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;

    private float pollingTime = 1f;
    private float zaman;
    private int frameCount;

    // Update is called once per frame
    void Update()
    {
        zaman += Time.deltaTime;
        frameCount++;

        if (zaman > pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / zaman);
            fpsText.text = frameRate.ToString() + " FPS";

            // disable to get average framerate
            zaman -= pollingTime;
            // disable to get average framerate
            frameCount = 0;
        }
    }