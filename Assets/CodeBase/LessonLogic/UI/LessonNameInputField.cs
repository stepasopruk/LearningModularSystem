﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class LessonNameInputField : MonoBehaviour
{
    [SerializeField] private SaveSettingLessonButton saveSettingLessonButton;
    
    private InputField _inputField;
    private Button _saveButton;

    private ILesson _lesson;

    public string Text 
    { 
        get; 
        set; 
    }

    private void OnEnable()
    {
        _inputField = GetComponent<InputField>();
        _inputField.text = Text;
        _saveButton = saveSettingLessonButton.GetComponent<Button>();
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable() => 
        _inputField.onValueChanged.RemoveListener(OnValueChanged);

    public void Inject(ILesson lesson)
    {
        _lesson = lesson;
    }

    private void OnValueChanged(string text)
    {
        if (text == string.Empty)
        {
            _saveButton.interactable = false;
            return;
        }
        _saveButton.interactable = true;
        _lesson.Name = text;
    }
}