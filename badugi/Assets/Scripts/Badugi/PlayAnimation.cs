using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayAnimation : MonoBehaviour
{

    public float playSpeed = 1;
    public List<Sprite> frames;
    public bool pass = false;

    int currentFrame = 0;
    float timer = 0;

    public void SetCurrentFrame(int frame)
    {
        currentFrame = frame;
        if (frame == 4)
            pass = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = frames[currentFrame];
    }
}