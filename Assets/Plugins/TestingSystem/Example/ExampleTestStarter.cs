using System.Linq;
using TestingSystem.Controllers;
using TestingSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace TestingSystem.Example
{
    public class ExampleTestStarter : MonoBehaviour
    {
        [SerializeField] private TestController testController;
        [SerializeField] private GameObject window;
        [SerializeField] private Text resultView;
        [SerializeField] private Button startTestButton;
        
        private readonly ExampleTest _exampleTest = new ExampleTest();
        
        private void Awake()
        {
            startTestButton.onClick.AddListener(StartTest);
            testController.OnTestFinished += Finish;
            resultView.text = string.Empty;
            window.SetActive(true);
        }

        private void OnDestroy()
        {
            startTestButton.onClick.RemoveListener(StartTest);
            testController.OnTestFinished -= Finish;
        }

        private void StartTest()
        {
            testController.StartTest(_exampleTest.Test);
            HideViews();
        }

        private void HideViews()
        {
            window.SetActive(false);
            startTestButton.gameObject.SetActive(false);
            resultView.gameObject.SetActive(false);
        }

        private void ShowResult(int gained, int total)
        {
            resultView.text = $"Result: {gained}/{total}";
            ShowViews();
        }

        private void ShowViews()
        {
            window.SetActive(true);
            resultView.gameObject.SetActive(true);
            startTestButton.gameObject.SetActive(true);
        }

        private void Finish(TestResult result)
        {
            int total = result.QuestionResults.Sum(questionResult => questionResult.Question.Weight);
            ShowResult(result.GetRating(), total);
        }
    }
}