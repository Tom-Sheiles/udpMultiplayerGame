﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{

    class CameraState
    {
        public float yaw;
        public float pitch;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw);
        }
    }

    CameraState targetState = new CameraState();
    CameraState InterpolateCameraState = new CameraState();
    GameObject playerObject;

    public bool invertY = false;
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    public float positionLerpTime = 0.2f;
    public float rotationLerpTime = 0.01f;

    void OnEnable()
    {
        targetState.SetFromTransform(transform);
        InterpolateCameraState.SetFromTransform(transform);
        playerObject = transform.parent.gameObject;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
            var mouseSenseFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            targetState.yaw += mouseMovement.x * mouseSenseFactor;
            targetState.pitch += mouseMovement.y * mouseSenseFactor;

            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            InterpolateCameraState.LerpTowards(targetState, positionLerpPct, rotationLerpPct);

            InterpolateCameraState.UpdateTransform(transform);

            var characterRotation = transform.rotation;
            characterRotation.x = 0;
            characterRotation.z = 0;

            playerObject.transform.rotation = characterRotation;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}