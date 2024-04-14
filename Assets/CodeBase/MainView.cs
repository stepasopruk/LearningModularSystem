using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject settingLessonPanel;
    [SerializeField] private LessonController lessonController;
    [SerializeField] private ListLessonView listLessonView;
    [SerializeField] private CreateLessonButton createLessonButton;
    [SerializeField] private Button loadLessonButton;
    [SerializeField] private Button startTrainingButton;

    private ILesson _lesson;

    public void Inject(ILesson lesson)
    {
        listLessonView.Inject(lesson, lessonController, this.gameObject, settingLessonPanel);
    }
}
