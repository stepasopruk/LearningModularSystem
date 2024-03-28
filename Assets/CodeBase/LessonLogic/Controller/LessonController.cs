using System.Collections.Generic;
using UnityEngine;

internal class LessonController : MonoBehaviour
{
    private List<Lesson> _lessons;
    public List<Lesson> Lessons => _lessons;

    private Lesson _currentLesson;
    public ILesson CurrentLesson => _currentLesson;

    private void Awake()
    {
        _lessons = new List<Lesson>();
    }

    public void AddLesson(Lesson lesson)
    {
        _lessons.Add(lesson);
        _currentLesson = lesson;
    }
}