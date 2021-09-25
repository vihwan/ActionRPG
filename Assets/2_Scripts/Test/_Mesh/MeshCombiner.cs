using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        
        if(CheckSampleMaterial(meshRenderers) == true)
        {
            CombineInstance[] combines = new CombineInstance[meshFilters.Length];
            int i = 0;
            while(i < meshFilters.Length)
            {
                combines[i].mesh = meshFilters[i].sharedMesh;
                combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = meshRenderers[0].sharedMaterial;
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combines);
            transform.gameObject.SetActive(true);
        }
    }

    private bool CheckSampleMaterial(MeshRenderer[] meshRenderers)
    {
        Material mtrl = meshRenderers[0].sharedMaterial;
        int i = 0;
        for (i = 1; i < meshRenderers.Length; i++)
        {
            if(mtrl != meshRenderers[i].sharedMaterial)
                return false;
        }

        return true;
    }
}
