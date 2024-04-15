using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class LessonNameInputField : MonoBehaviour
{
    [SerializeField] private Button saveSettingLessonButton;
    
    private InputField _inputField;

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
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable() => 
        _inputField.onValueChanged.RemoveListener(OnValueChanged);

    public void LessonChanged(ILesson lesson)
    {
        _lesson = lesson;
    }

    private void OnValueChanged(string text)
    {
        if (text == string.Empty)
        {
            saveSettingLessonButton.interactable = false;
            return;
        }
        saveSettingLessonButton.interactable = true;
        _lesson.Name = text;
    }
}