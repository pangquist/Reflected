using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private float startY;
    [SerializeField] private float amount = 50f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool startRandom = true;

    private float progress;

    private void Start()
    {
        if (startRandom)
            progress = Random.Range(0f, 1f);
    }

    private void Update()
    {
        progress += speed * Time.deltaTime;
        progress %= 1f;

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            startY + amount * Mathf.Sin(progress * Mathf.PI * 2f),
            transform.localPosition.z);
    }
}
