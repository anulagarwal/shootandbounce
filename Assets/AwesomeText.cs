using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AwesomeText : MonoBehaviour
{
    [SerializeField] TextMeshPro tm;
    [SerializeField] float riseSpeed;
    [SerializeField] float fadeSpeed;
    [SerializeField] float destroyDelay;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        tm.alpha -= fadeSpeed;
    }

    public void SetText(int s)
    {
        tm.text = "$"+ GameManager.Instance.FormatNumber(s); 
    }
}
