using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleLessonView : MonoBehaviour
{
    [SerializeField] private Text title;

    private Toggle toggle;
    private ILesson _lesson;

    public string Title
    {
        get => title.text;
        set => title.text = value;
    }

    private void OnDestroy()
    {
        _lesson.LessonNameChanged -= Lesson_LessonNameChanged;
    }

    public void Inject(ILesson lesson, ToggleGroup group)
    {
        toggle = GetComponent<Toggle>();
        _lesson = lesson;
        title.text = _lesson.Name;
        _lesson.LessonNameChanged += Lesson_LessonNameChanged;
        toggle.group = group;
    }

    private void Lesson_LessonNameChanged(string name)
    {
        title.text = name;
    }
}
