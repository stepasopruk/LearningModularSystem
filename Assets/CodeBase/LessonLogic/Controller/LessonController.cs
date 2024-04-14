using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILessonController
{
    public event Action<ILesson> ListLessonChanged;
    void SetLesson(ILesson lesson);
    void AddLesson(ILesson lesson);
    void RemoveLesson(ILesson lesson);
}

internal class LessonController : MonoBehaviour, ILessonController
{
    public event Action<ILesson> ListLessonChanged;

    [SerializeField] private LessonInjector lessonInjector;

    private List<Lesson> _lessons;
    public List<Lesson> Lessons => _lessons;

    private ILesson _currentLesson;
    public ILesson CurrentLesson => _currentLesson;

    private void Awake()
    {
        _lessons = new List<Lesson>();
    }

    public void SetLesson(ILesson lesson)
    {
        _currentLesson = lesson;
        lessonInjector.Inject(lesson);
    }

    public void AddLesson(ILesson lesson)
    {
        _lessons.Add((Lesson)lesson);
        SetLesson(lesson);
        ListLessonChanged?.Invoke(lesson);
    }

    public void RemoveLesson(ILesson lesson)
    {
        _lessons.Remove((Lesson)lesson);
        ListLessonChanged?.Invoke(lesson);
    }

    public void SetLessonName(string name) => 
        _currentLesson.Name = name;

    public void SaveSettingLesson()
    {
        //listLessonView.AddLessonList(_currentLesson);
        _currentLesson = null;
    }
}
