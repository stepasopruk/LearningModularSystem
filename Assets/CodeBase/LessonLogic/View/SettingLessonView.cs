using UnityEngine;
using UnityEngine.UI;

public class SettingLessonView : MonoBehaviour
{
    [SerializeField] private LessonNameInputField lessonNameInputField;
    [SerializeField] private ListModuleView listModuleView;

    public void Inject(ILessonController lessonController)
    {

    }

    public void LessonChanged(ILesson lesson)
    {
        lessonNameInputField.LessonChanged(lesson);
        lessonNameInputField.Text = lesson.Name;

        foreach (IModule module in lesson.LessonModules)
            listModuleView.AddModuleList(module);
    }
}
