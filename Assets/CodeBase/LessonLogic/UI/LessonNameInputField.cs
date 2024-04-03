using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class LessonNameInputField : MonoBehaviour
{
    [SerializeField] private LessonController lessonController;
    [SerializeField] private SaveSettingLessonButton saveSettingLessonButton;
    
    private InputField _inputField;
    private Button _saveButton;

    public string Text => _inputField.text;

    protected virtual void Awake()
    {
        _inputField = GetComponent<InputField>();
        _saveButton = saveSettingLessonButton.GetComponent<Button>();
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy() => 
        _inputField.onValueChanged.RemoveListener(OnValueChanged);

    private void OnValueChanged(string text)
    {
        if (text == string.Empty)
        {
            _saveButton.interactable = false;
            return;
        }
        _saveButton.interactable = true;
        lessonController.SetLessonName(text);
    }
}