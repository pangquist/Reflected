using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDecoration : MonoBehaviour
{
    [SerializeField] private GameObject normalVersion;
    [SerializeField] private List<GameObject> mechanicalVersions;

    public void Awake()
    {
        ObjectPlacer.Finished.AddListener(UpdateVersion);
    }

    private void UpdateVersion()
    {
        normalVersion.SetActive(false);

        foreach (GameObject mechanicalVersion in mechanicalVersions)
            mechanicalVersion.SetActive(false);

        Room room = transform.parent.parent.GetComponent<Room>();

        if (room.BossDistance < 200)
            mechanicalVersions.GetRandom().SetActive(true);
        else
            normalVersion.SetActive(true);
    }

}
