using System;
using UnityEngine;
using UserInterfaceExtension;

public class CreateLessonButton : AbstractButtonView
{
    [SerializeField] private LessonController lessonController;
    [SerializeField] private SettingLessonView settingLessonView;

    protected override void OnClick() => 
        CreateLesson();

    private void CreateLesson()
    {
        Lesson lesson = new Lesson();
        lessonController.AddLesson(lesson);
        settingLessonView.Initialize(lesson);
    }
}
