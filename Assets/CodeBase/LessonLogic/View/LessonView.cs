using System;
using UnityEngine;
using UnityEngine.UI;

public class LessonView : MonoBehaviour
{
    [SerializeField] private ToggleLessonView toggleLessonView;
    [SerializeField] private ListModuleLessonView listModuleLessonView;
    [SerializeField] private Button editButton;
    [SerializeField] private Button deleteButton;

    private Lesson _lesson;
    public Lesson Lesson => _lesson;

    private SettingLessonView _settingLessonView;

    private void Awake()
    {
        editButton.onClick.AddListener(EditLesson);
        deleteButton.onClick.AddListener(DeleteLesson);
    }

    private void OnDestroy()
    {
        editButton.onClick.RemoveListener(EditLesson);
        deleteButton.onClick.RemoveListener(DeleteLesson);
    }

    public void Initialize(Lesson lesson, ToggleGroup toggleGroupLessonView, SettingLessonView settingLessonView)
    {
        _lesson = lesson;
        _settingLessonView = settingLessonView;
        toggleLessonView.Initialize(lesson, toggleGroupLessonView);
        listModuleLessonView.Initialize(lesson.LessonModules);
    }

    private void EditLesson()
    {
        _settingLessonView.gameObject.SetActive(true);
        _settingLessonView.Initialize(_lesson);
    }

    private void DeleteLesson()
    {
        Destroy(gameObject);
    }
}