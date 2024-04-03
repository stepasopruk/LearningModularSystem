using UnityEngine;
using UserInterfaceExtension;

public class SaveSettingLessonButton : AbstractButtonView
{
    [SerializeField] private LessonController lessonController;

    protected override void OnClick() => 
        lessonController.SaveSettingLesson();
}
