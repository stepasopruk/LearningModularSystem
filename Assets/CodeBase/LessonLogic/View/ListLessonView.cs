using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListLessonView : MonoBehaviour
{
    public event Action<bool> ToggleActive;

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
        LessonView lessonView = Instantiate(lessonViewPrefab, content);
        lessonView.Initialize(lesson, toggleGroupLessonView);
        _lessonViews.Add(lessonView);
    }

    public void RemoveLessonList(Lesson lesson)
    {
        LessonView lessonView = _lessonViews.FirstOrDefault(x => x.Equals(lesson));
        _lessonViews.Remove(lessonView);
        Destroy(lessonView);
    }
}
