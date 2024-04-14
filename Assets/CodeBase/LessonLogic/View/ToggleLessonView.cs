using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleLessonView : MonoBehaviour
{
    [SerializeField] private Text title;

    private Toggle toggle;
    private ILesson _lesson;
    public ILesson Lesson => _lesson;

    public string Title
    {
        get => title.text;
        set => title.text = value;
    }

    public void Inject(ILesson lesson, ToggleGroup group)
    {
        toggle = GetComponent<Toggle>();
        _lesson = lesson;
        title.text = _lesson.Name;
        toggle.group = group;
    }
}
