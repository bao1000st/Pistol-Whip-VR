using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    public float value = 1;
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (value<1)
        {
            SetValue(value);
            value += 0.01f*speed;
        }
    }

    public void SetValue(float value)
    {
        var render = transform.GetChild(0).GetComponent<Renderer>();
        var materials = render.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
            //materials[i].SetVector("_DissolveOffest", new Vector4(0f,value,0f,0f));
        }
    }
}
