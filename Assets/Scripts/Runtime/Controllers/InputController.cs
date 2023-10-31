using System;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers
{
    public class InputController: MonoBehaviour
    {
        private Camera _mainCamera;
        private InputData _inputData;
        private bool _isAvailableTouch;
        
        private SwipeDirection _swipeDirection;
        private Vector2 _firstTouchPosition;
        private GameObject _targetDropObject;
        private bool _isRaycastSuccessful;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        internal void SetInputData(InputData data) => _inputData = data;
        internal void SetTouchAvailability(bool isAvailableForTouch) => _isAvailableTouch = isAvailableForTouch;
        
        private void Update()
        {
            if (!_isAvailableTouch)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                ProcessTouchStart();
            }
            
            if (!_isRaycastSuccessful)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                ProcessTouchEnd();
            }
        }

        private void ProcessTouchStart()
        {
            _firstTouchPosition = Input.mousePosition;
            _targetDropObject = GetRaycastTarget();
        }

        private void ProcessTouchEnd()
        {
            float swipeDeltaY = Input.mousePosition.y - _firstTouchPosition.y;
            float swipeDeltaX = Input.mousePosition.x - _firstTouchPosition.x;
            float swipeDistanceY = Math.Abs(swipeDeltaY);
            float swipeDistanceX = Math.Abs(swipeDeltaX);
            
            if (swipeDistanceY > swipeDistanceX)
            {
                // vertical swipe
                if (swipeDistanceY > _inputData.MinVerticalSwipeDistance
                    && swipeDistanceY < _inputData.MaxVerticalSwipeDistance)
                {
                    if (swipeDeltaY > 0)
                    {
                        _swipeDirection = SwipeDirection.Up;
                    }
                    else
                    {
                        _swipeDirection = SwipeDirection.Bottom;
                    }
                }
                else
                {
                    return;
                }
            }
            else if (swipeDistanceX > swipeDistanceY)
            {
                // horizontal swipe
                if (swipeDistanceX > _inputData.MinHorizontalSwipeDistance
                    && swipeDistanceX < _inputData.MaxHorizontalSwipeDistance)
                {
                    if (swipeDeltaX > 0)
                    {
                        _swipeDirection = SwipeDirection.Right;
                    }
                    else
                    {
                        _swipeDirection = SwipeDirection.Left;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                //Debug.LogWarning("There is no swipe");
                return;
            }
            
            InputSignals.Instance.OnInputDisable?.Invoke();
            InputSignals.Instance.OnInputTaken?.Invoke(new MatchInfoParams(_swipeDirection, _targetDropObject));
        }
        private GameObject GetRaycastTarget()
        {
            Vector2 mousePosInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

            var hitCollider = hit.collider;
            if (hitCollider != null && hitCollider.CompareTag("Drop"))
            {
                _isRaycastSuccessful = true;
                return hitCollider.gameObject;
            }
            
            _isRaycastSuccessful = false;
            return null;
        }
    }
}