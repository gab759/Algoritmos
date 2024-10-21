using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMapController : MonoBehaviour
{
    private SpriteRenderer _compSpriteRenderer;
    private void Awake()
    {
        _compSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetSprite(Sprite sprite)
    {
        _compSpriteRenderer.sprite = sprite;
    }
}
