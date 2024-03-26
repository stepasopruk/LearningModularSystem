using System.Linq;
using TestingSystem.Models;
using TestingSystem.Views;
using TestingSystem.Views.Questions;

namespace TestingSystem.Factories
{
    /// <summary>
    /// Class that responsible for question view.
    /// </summary>
    public class QuestionFactory
    {
        private readonly QuestionView[] _questionViews;
        
        private QuestionView _currentView;
        
        public QuestionFactory(QuestionView[] questionViews)
        {
            _questionViews = questionViews;
        }
        
        /// <summary>
        /// Configure text view.
        /// </summary>
        /// <param name="textQuestion">Text question</param>
        /// <returns>Abstract implementation</returns>
        public QuestionView ShowTextQuestion(TextQuestion textQuestion)
        {
            TryDisablePreviousView();
            
            var view = FindView<QuestionTextView>();
            view.SetQuestion(textQuestion.Text);
            RegisterAndEnableView(view);

            return view;
        }

        private void RegisterAndEnableView(QuestionTextView view)
        {
            view.SetPanelActive(true);
            _currentView = view;
        }

        private void TryDisablePreviousView()
        {
            if (_currentView != null)
                _currentView.SetPanelActive(false);
        }


        private T FindView<T>() where T : QuestionView => 
            _questionViews.First(x => x is T) as T;
    }
}