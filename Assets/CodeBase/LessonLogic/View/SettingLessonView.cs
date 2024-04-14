using UnityEngine;
using UnityEngine.UI;

public class SettingLessonView : MonoBehaviour
{
    [SerializeField] private LessonController lessonController;
    [SerializeField] private LessonNameInputField lessonNameInputField;
    [SerializeField] private ListModuleView listModuleView;

    private ILesson _lesson;

    public void Inject(ILesson lesson)
    {
        _lesson = lesson;
        lessonNameInputField.Text = lesson.Name;

        foreach (IModule module in lesson.LessonModules)
            listModuleView.AddModuleList(module);
    }
}
