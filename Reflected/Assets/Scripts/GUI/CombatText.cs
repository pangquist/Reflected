using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField] TMP_Text damageText;
    [SerializeField] float lifeTime;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDamageText(float damage)
    {
        damageText.text = damage.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed, 0);
    }
}
