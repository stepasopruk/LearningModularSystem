using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInterfaceExtension;

public class SwitchActiveButton : AbstractButtonView
{
    [SerializeField] private GameObject _inactive;
    [SerializeField] private GameObject _active;

    protected override void OnClick()
    {
        _inactive.SetActive(false);
        _active.SetActive(true);
    }
}
