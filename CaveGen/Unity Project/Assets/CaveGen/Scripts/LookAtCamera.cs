using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera camera;
    public bool doOnUpdate = true;
    public bool lookOpposite = true;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

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
        if (!rend.isVisible) return;

        if (lookOpposite) {
            transform.LookAt(2 * transform.position - camera.transform.position);
        } else {
            transform.LookAt(camera.transform);
        }
    }

    // If it gets too inefficient try using this...
    private bool IsVisible(GameObject Object, Collider collider) {
          Plane[] planes = GeometryUtility.CalculateFrustumPlanes(this.camera);
          if (GeometryUtility.TestPlanesAABB(planes , collider.bounds))
              return true;
          else
              return false;
     }
}
