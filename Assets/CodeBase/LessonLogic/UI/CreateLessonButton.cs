using System;
using UnityEngine;
using UserInterfaceExtension;

public class CreateLessonButton : AbstractButtonView
{
    private ILessonController _lessonController;

    private GameObject _active;
    private GameObject _unactive;

    public void Inject(ILessonController lessonController, GameObject active, GameObject unactive)
    {
        _lessonController = lessonController;
        _active = active;
        _unactive = unactive;
    }

    protected override void OnClick() => 
        CreateLesson();

    private void CreateLesson()
    {
        Lesson lesson = new Lesson();
        _lessonController.AddLesson(lesson);
        _active.SetActive(true);
        _unactive.SetActive(false);
    }
}
