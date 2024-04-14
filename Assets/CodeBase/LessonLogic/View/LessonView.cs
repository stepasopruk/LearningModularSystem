using System;
using UnityEngine;
using UnityEngine.UI;

public class LessonView : MonoBehaviour
{
    [SerializeField] private ToggleLessonView toggleLessonView;
    [SerializeField] private ListModuleLessonView listModuleLessonView;
    [SerializeField] private Button editButton;
    [SerializeField] private Button deleteButton;

    private ILesson _lesson;
    private ILessonController _lessonController;

    private GameObject _active;
    private GameObject _unactive;

    private void OnDestroy()
    {
        _lesson.ModelListChanged -= Lesson_ModelListChanged;
    }

    public void SubscribeOnClickButtons(GameObject active, GameObject unactive)
    {
        _active = active;
        _unactive = unactive;
        editButton.onClick.AddListener(Editlesson);
        deleteButton.onClick.AddListener(DeleteLesson);
    }

    public void UnSubscribeOnClickButtons()
    {
        editButton.onClick.RemoveListener(Editlesson);
        deleteButton.onClick.RemoveListener(DeleteLesson);
    }

    public void Inject(ILesson lesson, ILessonController lessonController, ToggleGroup toggleGroupLessonView)
    {
        _lesson = lesson;
        _lessonController = lessonController;
        _lesson.ModelListChanged += Lesson_ModelListChanged;
        toggleLessonView.Inject(lesson, toggleGroupLessonView);
    }

    private void Lesson_ModelListChanged(IModule module)
    {
        listModuleLessonView.UpdateListModules(module);
    }

    private void Editlesson()
    {
        _lessonController.SetLesson(_lesson);
        _active.SetActive(true);
        _unactive.SetActive(false);
    }

    private void DeleteLesson()
    {
        _lessonController.RemoveLesson(_lesson);
        Destroy(gameObject);
    }
}