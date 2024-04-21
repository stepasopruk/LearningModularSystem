using UnityEngine;
using UnityEngine.UI;

public abstract class ModuleViewBase : MonoBehaviour
{
    [SerializeField] protected Text title;

    protected ModuleSO _moduleSO;

    public string Title => title.text;

    public void RecordingDataView(ModuleSO moduleSO)
    {
        _moduleSO = moduleSO;
        title.text = moduleSO.ModuleName;
        SetDataView();
    }

    protected abstract void SetDataView();
}
