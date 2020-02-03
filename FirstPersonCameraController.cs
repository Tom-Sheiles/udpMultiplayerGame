using System.Collections;
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
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw);
        }
    }

    [SerializeField] GameObject settings;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject Back;

    CameraState targetState = new CameraState();
    CameraState InterpolateCameraState = new CameraState();
    GameObject playerObject;
    float RotY;
    float RotX;

    public bool invertY = false;
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    public float positionLerpTime = 0.2f;
    public float rotationLerpTime = 0.01f;
    public float mouseSenseValue = 1.5f;

    void OnEnable()
    {
        targetState.SetFromTransform(transform);
        InterpolateCameraState.SetFromTransform(transform);
        playerObject = transform.parent.gameObject;
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(settings.activeInHierarchy) { settings.SetActive(false); QuitButton.SetActive(false); }
            else { settings.SetActive(true); QuitButton.SetActive(true); }
            Back.SetActive(false);
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            RotX = Input.GetAxis("Mouse X");
            RotY = Input.GetAxis("Mouse Y");

            var mouseMovement = new Vector2(RotX, RotY * (invertY ? 1 : -1));
            //var mouseSenseFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude) * mouseSenseValue;
            var mouseSenseFactor = mouseSenseValue;

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
