using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] float turnSmoothTime;
    private float turnSmoothVelocity;


    //---------------------------------------------
    CinemachineFreeLook freeLook;
    Transform playerTransform;
    Transform currentTransform;
    private void Awake()
    {
        freeLook = FindObjectOfType<CinemachineFreeLook>();
    }
    public void FocusOnPlayer()
    {
        currentTransform = playerTransform;
        freeLook.LookAt = currentTransform;
    }
    public void NewFocus(Transform newTransform)
    {
        currentTransform = newTransform;
        freeLook.LookAt = currentTransform;
    }

    public void BossSettings()
    {
        freeLook.m_Orbits[0].m_Height = 5;
        freeLook.m_Orbits[0].m_Radius = 25;

        freeLook.m_Orbits[1].m_Height = 5;
        freeLook.m_Orbits[1].m_Radius = 25;

        freeLook.m_Orbits[2].m_Height = 5;
        freeLook.m_Orbits[2].m_Radius = 25;
    }

    public void Rotate(Vector2 mousePosition)
    {
        float targetAngle = Mathf.Atan2(mousePosition.x, mousePosition.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public Transform Focus() => currentTransform;
}
