using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Indicator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Indicator.transform.position + new Vector3(0, 1, 0);
    }
}
