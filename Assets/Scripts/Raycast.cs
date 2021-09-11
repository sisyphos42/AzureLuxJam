using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    public GameObject text;
    TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = text.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        // RaycastHit hit;
        // Collider coll = GetComponent<Collider>();
        // if (coll.Raycast(ray, out hit, 100f)) {
        //     Debug.Log(hit.textureCoord.x);
        // }

         RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        if (Input.GetMouseButtonDown(0)) {
            if (hit.collider.tag == "Satellite") {
                hit.collider.GetComponent<SatOrbit>().SetInfected(false);

            }
        }

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;



        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        Color pix = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        _text.color = pix;

        //tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
        //tex.Apply();
    }
}
