using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListLessonView : MonoBehaviour
{
    public event Action<bool> ToggleActive;

    [SerializeField] private Transform content;
    [SerializeField] private ToggleGroup toggleGroupLessonView;
    [SerializeField] private LessonView lessonViewPrefab;

    private ILessonController _lessonController;
    private List<ILesson> _lessons;
    private List<LessonView> _lessonViews;

    private GameObject _active;
    private GameObject _unactive;

    private void Awake()
    {
        _lessons = new List<ILesson>();
        _lessonViews = new List<LessonView>();
    }

    private void OnDestroy()
    {
        if(_lessonController != null)
            _lessonController.ListLessonChanged -= LessonController_ListLessonChanged;

        foreach (LessonView lessonView in _lessonViews)
            lessonView.UnSubscribeOnClickButtons();
    }

    public void Inject(ILesson lesson, ILessonController lessonController, GameObject active, GameObject unactive)
    {
        _active = active;
        _unactive = unactive;
        _lessonController = lessonController;
        _lessonController.ListLessonChanged += LessonController_ListLessonChanged;
    }

    private void LessonController_ListLessonChanged(ILesson lesson)
    {
        if (!_lessons.Any(x => x == lesson))
            AddLessonList(lesson);
        else
            RemoveLessonList(lesson);
    }

    private void AddLessonList(ILesson lesson)
    {
        LessonView lessonView = Instantiate(lessonViewPrefab, content);
        lessonView.Inject(lesson, _lessonController, toggleGroupLessonView);
        lessonView.SubscribeOnClickButtons(_active, _unactive);
        _lessonViews.Add(lessonView);
    }

    private void RemoveLessonList(ILesson lesson)
    {
        LessonView lessonView = _lessonViews.FirstOrDefault(x => x.Equals(lesson));
        lessonView.UnSubscribeOnClickButtons();
        _lessonViews.Remove(lessonView);
        Destroy(lessonView);
    }
}
