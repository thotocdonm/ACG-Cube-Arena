using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void LateUpdate()
    {
        if (!cam) return;

        Vector3 dir = transform.position - cam.transform.position;
        dir.x = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
