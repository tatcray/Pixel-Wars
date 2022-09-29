using System.Collections.Generic;
using Dependencies;
using Extensions;
using Services;

namespace Tutorial
{
    public class TutorialManager
    {
        private List<ITutorialPart> tutorials = new List<ITutorialPart>();
        private ObservableSerializedObject<int> currentTutorialPart;

        public TutorialManager(ObservableSerializedObject<int> tutorialIndex)
        {
            currentTutorialPart = tutorialIndex;
        }
        
        public void AddTutorialPart(ITutorialPart part)
        {
            tutorials.Add(part);
        }

        public bool IsFinished()
        {
            return tutorials.Count - 1 < currentTutorialPart.Value;
        }

        public void StartTutorial()
        {
            if (IsFinished())
                return;

            currentTutorialPart.Value = 0;
            SetupCurrentTutorial();
        }

        private void RunNextTutorial()
        {
            AnalyticsController.SendTutorialPartComplete(currentTutorialPart.Value);
            currentTutorialPart.Value++;
            if (IsFinished())
                return;
            
            SetupCurrentTutorial();
        }

        private void SetupCurrentTutorial()
        {
            tutorials[currentTutorialPart.Value].StartTutorial(RunNextTutorial);
        }
    }
}
