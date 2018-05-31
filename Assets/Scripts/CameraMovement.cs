using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject MapCamera;

    public void Update()
    {
        // Keybord controlls.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            ZoomIn();
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            ZoomOut();
        }
    }

    public void MoveUp()
    {
        MapCamera.transform.Translate(0f, 1f, 0f);
    }

    public void MoveDown()
    {
        MapCamera.transform.Translate(0f, -1f, 0f);
    }

    public void MoveLeft()
    {
        MapCamera.transform.Translate(-1f, 0f, 0f);
    }

    public void MoveRight()
    {
        MapCamera.transform.Translate(1f, 0f, 0f);
    }

    public void ZoomIn()
    {
        MapCamera.transform.Translate(0f, 0f, 1f);
    }

    public void ZoomOut()
    {
        MapCamera.transform.Translate(0f, 0f, -1f);
    }
}
