using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject settingLessonPanel;
    [SerializeField] private ListLessonView listLessonView;
    [SerializeField] private CreateLessonButton createLessonButton;
    [SerializeField] private Button loadLessonButton;
    [SerializeField] private Button startTrainingButton;

    public void Inject(ILessonController lessonController)
    {
        listLessonView.Inject(lessonController, settingLessonPanel, this.gameObject);
        createLessonButton.Inject(lessonController, settingLessonPanel, this.gameObject);
    }

    public void LessonChanged(ILesson lesson)
    {
        listLessonView.LessonChanged(lesson);
    }
}
