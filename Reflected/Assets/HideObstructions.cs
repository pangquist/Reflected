using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstructions : MonoBehaviour
{
    [SerializeField] List<GameObject> currentlyInTheWay;
    [SerializeField] List<GameObject> alreadyHidden;
    [SerializeField] Transform player;
    private Transform camera;

    private void Awake()
    {
        camera = this.gameObject.transform;
    }

    void Update()
    {
        GetAllObjectsInTheWay();
        ShowObjects();
        HideObjects();
    }

    private void GetAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();

        float cameraPlayerDistance = Vector3.Magnitude(camera.position - player.position);

        Ray rayForward = new Ray(camera.position, player.position - camera.position);
        Ray rayBackward = new Ray(player.position, camera.position - player.position);

        var hitsForward = Physics.RaycastAll(rayForward, cameraPlayerDistance);
        var hitsBackward = Physics.RaycastAll(rayBackward, cameraPlayerDistance);

        foreach(var hit in hitsForward)
        {
            if(hit.collider.gameObject.TryGetComponent(out MeshRenderer mesh) && hit.collider.gameObject.tag == "Decoration")
            {
                if (!currentlyInTheWay.Contains(hit.collider.gameObject))
                {
                    currentlyInTheWay.Add(hit.collider.gameObject);
                }
            }
        }

        foreach (var hit in hitsBackward)
        {
            if (hit.collider.gameObject.TryGetComponent(out MeshRenderer mesh) && hit.collider.gameObject.tag == "Decoration")
            {
                if (!currentlyInTheWay.Contains(hit.collider.gameObject))
                {
                    currentlyInTheWay.Add(hit.collider.gameObject);
                }
            }
        }
    }

    private void HideObjects()
    {
        for (int i = 0; i < currentlyInTheWay.Count; i++)
        {
            if(currentlyInTheWay[i].TryGetComponent(out MeshRenderer mesh))
            {
                if (!alreadyHidden.Contains(currentlyInTheWay[i]))
                {
                    mesh.enabled = false;
                    alreadyHidden.Add(currentlyInTheWay[i]);
                }
            }
        }
    }

    private void ShowObjects()
    {
        for (int i = alreadyHidden.Count - 1; i >= 0; i--)
        {
            if (alreadyHidden[i].TryGetComponent(out MeshRenderer mesh))
            {
                if (!currentlyInTheWay.Contains(alreadyHidden[i]))
                {
                    alreadyHidden.Remove(alreadyHidden[i]);
                    mesh.enabled = true;
                    //StartCoroutine(HideTimer(alreadyHidden[i]));
                }
            }
        }
    }

    IEnumerator HideTimer(GameObject gameObject)
    {
        alreadyHidden.Remove(gameObject);

        yield return new WaitForSeconds(1f);

        StopHide(gameObject);
    }

    public void StopHide(GameObject gameObject)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        StopCoroutine(HideTimer(gameObject));
    }
}
