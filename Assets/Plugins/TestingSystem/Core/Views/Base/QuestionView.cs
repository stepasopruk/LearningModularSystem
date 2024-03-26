using UnityEngine;
using UnityEngine.UI;

namespace TestingSystem.Views
{
    /// <summary>
    /// Base implementation for Question view.
    /// </summary>
    public abstract class QuestionView : MonoBehaviour
    {
        public Transform Container => container;
        public Button ConfirmButton => confirmButton;

        [SerializeField] private Button confirmButton;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject panel;
        
        /// <summary>
        /// Show or hide panel by bool.
        /// </summary>
        /// <param name="active">true - display, false - hide</param>
        public void SetPanelActive(bool active) => 
            panel.SetActive(active);
    }
}