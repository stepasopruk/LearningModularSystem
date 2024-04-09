using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal class LessonController : MonoBehaviour
{
    [SerializeField] private ListLessonView listLessonView;

    private List<Lesson> _lessons;
    public List<Lesson> Lessons => _lessons;

    private Lesson _currentLesson;
    public ILesson CurrentLesson => _currentLesson;

    private void Awake()
    {
        _lessons = new List<Lesson>();
    }

    public void SetLesson(Lesson lesson)
    {
        _currentLesson = lesson;
    }

    public void AddLesson(Lesson lesson)
    {
        _lessons.Add(lesson);
        _currentLesson = lesson;
    }

    public void RemoveLesson(Lesson lesson)
    {
        _lessons.Remove(lesson);
        listLessonView.RemoveLessonList(lesson);
    }

    public void SetLessonName(string name) => 
        _currentLesson.Name = name;

    public void SaveSettingLesson()
    {
        listLessonView.AddLessonList(_currentLesson);
        _currentLesson = null;
    }
}
