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

    public void RemoveLesson(Lesson lesson)
    {
        _lessons.Remove(lesson);
    }

    public void SetLessonName(string name) => 
        _currentLesson.Name = name;

    public void SaveSettingLesson()
    {
        //TODO: invoke event LessonSaved and add Lesson to DataBase
        _currentLesson = null;
    }
}