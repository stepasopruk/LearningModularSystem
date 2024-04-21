using UnityEngine;
using UnityEngine.UI;
// Создание пустого модуля
// Нажатие на редактирование
// Список шаблонов заполненных модулей
// Редактор модуля (пока будет пусто)

[RequireComponent(typeof(Button))]
public class ModuleViewItem : MonoBehaviour
{
    [SerializeField] private Text title;

    private Button _button;
    public Button Button => _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Initialize(string title)
    {
        this.title.text = title; 
    }
}