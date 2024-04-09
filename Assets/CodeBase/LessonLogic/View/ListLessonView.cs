using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListLessonView : MonoBehaviour
{
    public event Action<bool> ToggleActive;

    [SerializeField] private SettingLessonView settingLessonView;
    [SerializeField] private Transform content;
    [SerializeField] private ToggleGroup toggleGroupLessonView;
    [SerializeField] private LessonView lessonViewPrefab;

    private List<LessonView> _lessonViews;

    private void Awake()
    {
        _lessonViews = new List<LessonView>();
    }

    public void AddLessonList(Lesson lesson)
    {
        LessonView lessonView = new LessonView();
        lessonView = _lessonViews.FirstOrDefault(x => x.Lesson == lesson);

        if (lessonView != null)
        {
            lessonView.Initialize(lesson, toggleGroupLessonView, settingLessonView);
            return;
        }
          
        lessonView = Instantiate(lessonViewPrefab, content);
        lessonView.Initialize(lesson, toggleGroupLessonView, settingLessonView);
        _lessonViews.Add(lessonView);
    }

    public void RemoveLessonList(Lesson lesson)
    {
        LessonView lessonView = _lessonViews.FirstOrDefault(x => x.Equals(lesson));
        _lessonViews.Remove(lessonView);
        Destroy(lessonView);
    }
}
