using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonInjector : MonoBehaviour
{
    [SerializeField] private MainView mainView;
    [SerializeField] private SettingLessonView settingLessonView;

    private ILesson _lesson;

    public void Inject(ILesson currentLesson) 
    {
        _lesson = currentLesson;
        mainView.Inject(currentLesson);
        settingLessonView.Inject(currentLesson);
    }
}
