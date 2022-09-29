using System;
using System.Collections;
using Controller;
using Core;
using Dependencies;
using DG.Tweening;
using UnityEngine;

namespace Tutorial
{
    public class ShootTutorial : ITutorialPart
    {
        private Action completedCallback;
        private TutorialDependencies dependencies;
        private Crosshair crosshair;
        private Vector2 origin;
        private Sequence handAnimation;
        
        public ShootTutorial(TutorialDependencies dependencies, Crosshair crosshair)
        {
            this.crosshair = crosshair;
            this.dependencies = dependencies;
        }
        
        public void StartTutorial(Action completedCallback)
        {
            this.completedCallback = completedCallback; 
            
            StartHandAnimation();
            GameEvents.GameEndedByLose.Event += FinishTutorial;
            GameEvents.GameEndedByWin.Event += FinishTutorial;

            crosshair.ShootStarted += StopHandAnimation;
            crosshair.ShootStopped += StartHandAnimation;
        }

        private void FinishTutorial()
        {
            GameEvents.GameEndedByLose.Event -= FinishTutorial;
            GameEvents.GameEndedByWin.Event -= FinishTutorial;
            
            crosshair.ShootStarted -= StopHandAnimation;
            crosshair.ShootStopped -= StartHandAnimation;

            StopHandAnimation();
            completedCallback();
        }

        private void StopHandAnimation()
        {
            handAnimation?.Kill();
            dependencies.handContainer.color = Color.white;
            dependencies.handContainer.gameObject.SetActive(false);
        }

        private void StartHandAnimation()
        {
            origin = crosshair.transform.anchoredPosition;
            dependencies.handContainer.gameObject.SetActive(true);

            Vector2 right = origin + Vector2.right * dependencies.pixelsHandWalkRange;
            Vector2 left = origin + Vector2.left * dependencies.pixelsHandWalkRange;
            Vector2 bottom = origin + Vector2.down * dependencies.pixelsHandWalkRange;
            Vector2 top = origin + Vector2.up * dependencies.pixelsHandWalkRange;

            dependencies.handContainer.sprite = dependencies.idleHand;
            dependencies.handContainer.rectTransform.anchoredPosition = origin;

            dependencies.handContainer.color = new Color(1, 1, 1, 0);
            handAnimation = DOTween.Sequence()
                .Append(dependencies.handContainer.DOFade(1f, dependencies.fadeDuration))
                .AppendInterval(dependencies.handIdleDuration)
                .AppendCallback(() => dependencies.handContainer.sprite = dependencies.pressedHand)
                .Append(dependencies.handContainer.rectTransform.DOAnchorPos(top, dependencies.handMoveDuration))
                .AppendInterval(dependencies.handMoveDelay)
                .Append(dependencies.handContainer.rectTransform.DOAnchorPos(right, dependencies.handMoveDuration))
                .AppendInterval(dependencies.handMoveDelay)
                .Append(dependencies.handContainer.rectTransform.DOAnchorPos(left, dependencies.handMoveDuration))
                .AppendInterval(dependencies.handMoveDelay)
                .Append(dependencies.handContainer.rectTransform.DOAnchorPos(bottom, dependencies.handMoveDuration))
                .AppendInterval(dependencies.handMoveDelay)
                .Append(dependencies.handContainer.rectTransform.DOAnchorPos(origin, dependencies.handMoveDuration))
                .AppendCallback(() => dependencies.handContainer.sprite = dependencies.idleHand)
                .Append(dependencies.handContainer.DOFade(0, dependencies.fadeDuration))
                .AppendInterval(dependencies.handIdleDuration)
                .OnUpdate(() => crosshair.transform.anchoredPosition = dependencies.handContainer.rectTransform.anchoredPosition)
                .SetLoops(-1);
        }
    }
}