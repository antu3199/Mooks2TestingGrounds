using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMesh : MonoBehaviour
{

    public MeshFilter myMeshFilter;

    public FogOfWarScript fogScript; // tmp

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myMeshFilter.mesh = fogScript.m_mesh;
    }
}
