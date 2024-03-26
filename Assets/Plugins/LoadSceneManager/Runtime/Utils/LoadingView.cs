using UnityEngine;
using UnityEngine.UI;

namespace LoadSceneManager.Utils
{
    public class LoadingView : MonoBehaviour
    {
        private const string LOADING = "Загрузка";
        private const string DOT = ".";
        private const float UPDATE_TIME = 0.3f;
        private const int DOT_COUNT = 3;

        [SerializeField] private Text view;

        private float _time;
        private int _dots;

        private void Update()
        {
            _time += Time.deltaTime;
            
            if (_time >= UPDATE_TIME)
            {
                _time = 0;
                UpdateDot();
            }
        }


        private void UpdateDot()
        {
            if (_dots == DOT_COUNT)
            {
                _dots = 0;
                view.text = LOADING;
            }
            else
            {
                _dots++;
                view.text += DOT;
            }
        }
    }
}