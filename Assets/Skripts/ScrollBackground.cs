using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float scrollSpeed;

    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(0f, Time.time * scrollSpeed);
    }
}
