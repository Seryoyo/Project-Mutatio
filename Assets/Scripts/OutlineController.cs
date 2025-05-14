using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    // Reference to the shader material defined in the next section
    public Material outlineMaterial;
    public float outlineSize = 1f;

    private List<Material> attachedMaterials = new List<Material>();

    void Start()
    {
        foreach (var s in GetComponentsInChildren<SpriteRenderer>())
        {
            AddOutline(s);
        }
    }

    private void AddOutline(SpriteRenderer sprite)
    {
        var width = sprite.bounds.size.x;
        var height = sprite.bounds.size.y;

        var widthScale = 1 / width;
        var heightScale = 1 / height;

        var outline = new GameObject("Outline");
        outline.transform.parent = sprite.gameObject.transform;
        outline.transform.localScale = new Vector3(1f, 1f, 1f);
        outline.transform.localPosition = new Vector3(0f, 0f, 0f);
        outline.transform.localRotation = Quaternion.identity;

        var outlineSprite = outline.AddComponent<SpriteRenderer>();
        outlineSprite.sprite = sprite.sprite;
        outlineSprite.material = outlineMaterial;

        outlineSprite.material.SetFloat("_HSize", 0.1f * widthScale * outlineSize);
        outlineSprite.material.SetFloat("_VSize", 0.1f * heightScale * outlineSize);
        outlineSprite.material.SetFloat("_Alpha", 1.0f); // Always fully visible

        outlineSprite.sortingOrder = -10;

        attachedMaterials.Add(outlineSprite.material);
    }

}

