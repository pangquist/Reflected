using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
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

    public Transform Focus() => currentTransform;
}
