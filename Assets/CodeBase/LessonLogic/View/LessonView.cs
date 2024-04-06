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

    private void Awake()
    {
        editButton.onClick.AddListener(EditLesson);
        deleteButton.onClick.AddListener(DeleteLesson);
    }

    public void Initialize(Lesson lesson, ToggleGroup toggleGroupLessonView)
    {
        _lesson = lesson;
        toggleLessonView.Initialize(lesson, toggleGroupLessonView);
        listModuleLessonView.Initialize(lesson.LessonModules);
    }

    private void EditLesson()
    {
        //TODO: Open window edit lesson 
    }

    private void DeleteLesson()
    {
        //TODO: Delete lesson 
    }
}