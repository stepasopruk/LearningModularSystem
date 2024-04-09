using UnityEngine;
using UnityEngine.UI;

public class SettingLessonView : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private LessonController lessonController;
    [SerializeField] private InputField nameLesson;
    [SerializeField] private ListModuleView listModuleView;

    public void Initialize(Lesson lesson)
    {
        mainPanel.SetActive(false);
        gameObject.SetActive(true);

        lessonController.SetLesson(lesson);
        nameLesson.text = lesson.Name;

        foreach (IModule module in lesson.LessonModules)
            listModuleView.AddModuleList(module);
    }

}
