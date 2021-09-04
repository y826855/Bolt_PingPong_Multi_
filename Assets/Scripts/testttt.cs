using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testttt : MonoBehaviour
{

    MeshRenderer m_renderer = null;
    Material m_mat = null;

    void Start()
    {
        m_mat = this.GetComponent<Renderer>().material;
        
        m_mat.SetColor("_AlbedoColor", new Color(1f, 1f, 1f, 1f));
        m_mat.SetColor("_TestEmission", new Color(1f, 1f, 1f));
        //m_renderer = this.GetComponent<MeshRenderer>();

        //m_renderer.material.SetColor("AlbedoColor", new Color(255, 255, 0, 255));
        //m_renderer.material.SetColor("TestEmission", new Color(255, 255, 0));

        //MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        //mpb.SetColor(Shader.PropertyToID("AlbedoColor"), Color.red);
        //m_renderer.SetPropertyBlock(mpb);
        //m_renderer.SetPropertyBlock(null);
    }

    // Update is called once per frame
    void Update()
    {
    }

    
}
