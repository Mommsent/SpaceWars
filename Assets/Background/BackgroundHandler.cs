using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    public float scrollSpeed = 0.2f;
    private MeshRenderer _backgroundRenderer;

    private void Start()
    {
        _backgroundRenderer = background.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        MoveBackground(_backgroundRenderer);
    }

    private void MoveBackground(MeshRenderer background)
    {
        background.material.mainTextureOffset = new Vector2(0f, Time.time * scrollSpeed);
    }
}
