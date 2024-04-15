using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonInjector : MonoBehaviour
{
    [SerializeField] private LessonController lessonController;
    [SerializeField] private MainView mainView;
    [SerializeField] private SettingLessonView settingLessonView;

    private void Awake() =>
        lessonController.CurrentLessonChanged += LessonController_CurrentLessonChanged;

    private void Start()
    {
        Inject();
    }

    private void OnDestroy() =>
        lessonController.CurrentLessonChanged -= LessonController_CurrentLessonChanged;

    private void Inject()
    {
        mainView.Inject(lessonController);
        settingLessonView.Inject(lessonController);
    }

    private void LessonController_CurrentLessonChanged(ILesson currentLesson)
    {
        mainView.LessonChanged(currentLesson);
        settingLessonView.LessonChanged(currentLesson);
    }
}
