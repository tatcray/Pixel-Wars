using System;

namespace Tutorial
{
    public interface ITutorialPart
    {
        public void StartTutorial(Action completedCallback);
    }
}