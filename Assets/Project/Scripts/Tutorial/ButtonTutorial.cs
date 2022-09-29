using System;
using System.Collections;
using Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class ButtonTutorial : ITutorialPart
    {
        protected TutorialDependencies dependencies;
        
        private Action completedCallback;
        private Coroutine activeCoroutine;
        private Button button;
        
        public ButtonTutorial(TutorialDependencies dependencies, Button button)
        {
            this.dependencies = dependencies;
            this.button = button;
        }
        
        public virtual void StartTutorial(Action completedCallback)
        {
            dependencies.handContainer.rectTransform.position = button.transform.position;
            button.onClick.AddListener(CompleteTutorial);

            activeCoroutine = CoroutinesHolder.StartCoroutine(HandClickAnimation());
            this.completedCallback = completedCallback;
        }

        protected  virtual void CompleteTutorial()
        {
            button.onClick.RemoveListener(CompleteTutorial);
            CoroutinesHolder.StopCoroutine(activeCoroutine);
            dependencies.handContainer.gameObject.SetActive(false);
            
            completedCallback();
        }

        private IEnumerator HandClickAnimation()
        {
            dependencies.handContainer.gameObject.SetActive(true);
            while (true)
            {
                dependencies.handContainer.sprite = dependencies.idleHand;
                yield return new WaitForSeconds(dependencies.clickDelay);
                dependencies.handContainer.sprite = dependencies.pressedHand;
                yield return new WaitForSeconds(dependencies.clickDelay);
            }
        }
    }
}