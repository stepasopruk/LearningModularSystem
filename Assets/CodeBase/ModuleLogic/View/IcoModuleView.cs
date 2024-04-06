using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcoModuleView : MonoBehaviour
{
    public Sprite Sprite { get; set; }

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = Sprite;
    }
}
