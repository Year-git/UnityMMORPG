using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Entities;

public class PlayerCamera : MonoSingleton<PlayerCamera>
{
    public GameObject player;
    public float rotateSpeed = 70.0f;
    public float rotateMinY = -50f;
    public float rotateMaxY = 30f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private void LateUpdate()
    {
        if (player == null)
            return;

        this.transform.position = player.transform.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseX != 0 || mouseY != 0)
        {
            xRotation -= mouseX * rotateSpeed * 0.02f;
            yRotation += mouseY * rotateSpeed * 0.02f;
            yRotation = ClampValue(yRotation, rotateMinY, rotateMaxY);

            Quaternion rotation = Quaternion.Euler(-yRotation, -xRotation, 0);
            this.transform.rotation = rotation;
        }
    }

    float ClampValue(float value, float min, float max)
    {
        if (value < -360)
            value += 360;
        if (value > 360)
            value -= 360;
        return Mathf.Clamp(value, min, max);
    }
}
