using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera camera;
    public bool doOnUpdate = true;
    public bool lookOpposite = true;
    // Start is called before the first frame update
    void Start()
    {
        if (camera == null) {
            camera = Camera.main;
        }
        
        this.LookAtTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (doOnUpdate) {
            this.LookAtTarget();
        }
    }

    void LookAtTarget() {
        if (lookOpposite) {
            transform.LookAt(2 * transform.position - camera.transform.position);
        } else {
            transform.LookAt(camera.transform);
        }
    }
}
