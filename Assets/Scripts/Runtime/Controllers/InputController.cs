using System;
using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Controllers
{
    public class InputController: MonoBehaviour
    {
        private bool _isAvailableTouch;
        private InputData _inputData;
        
        private SwipeDirection _swipeDirection;
        private GameObject _targetDropObject;
        
        private Vector2 _firstTouchPosition;
        private Ray _ray;
        private RaycastHit _hit;
        private bool _isRaycastSuccesful;
        internal void SetInputData(InputData data) => _inputData = data;
        internal void SetTouchAvailability(bool isAvailableForTouch) => _isAvailableTouch = isAvailableForTouch;
        
        private void Update()
        {
            if (!_isAvailableTouch)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogWarning("Mouse button DOWN");
                ProcessTouchStart();
            }
            
            if (!_isRaycastSuccesful)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                Debug.LogWarning("Mouse button UP");
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
            float verticalSwipeDelta = Input.mousePosition.y - _firstTouchPosition.y;
            float horizontalSwipeDelta = Input.mousePosition.x - _firstTouchPosition.x;
            
            if (verticalSwipeDelta > horizontalSwipeDelta)
            {
                float distance = Math.Abs(verticalSwipeDelta);
                
                if (distance > _inputData.VerticalSwipeDistanceMin
                    && distance < _inputData.VerticalSwipeDistanceMax)
                {
                    // vertical swipe

                    if (verticalSwipeDelta > 0)
                    {
                        _swipeDirection = SwipeDirection.Right;
                    }
                    else
                    {
                        _swipeDirection = SwipeDirection.Left;
                    }
                }
            }
            else if (horizontalSwipeDelta > verticalSwipeDelta)
            {
                float distance = Math.Abs(horizontalSwipeDelta);
                
                if (distance > _inputData.HorizontalSwipeDistanceMin
                    && distance < _inputData.HorizontalSwipeDistanceMax)
                {
                    // horizontal swipe
                        
                    if (horizontalSwipeDelta > 0)
                    {
                        _swipeDirection = SwipeDirection.Up;
                    }
                    else
                    {
                        _swipeDirection = SwipeDirection.Bottom;
                    }
                }
            }
            else
            {
                //Debug.LogWarning("There is no swipe");
                return;
            }
            
            InputSignals.Instance.onInputDisable?.Invoke();
            InputSignals.Instance.onInputTaken?.Invoke(new MatchInfoParams(_swipeDirection, _targetDropObject));
        }
        private GameObject GetRaycastTarget()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    GameObject targetObject = result.gameObject;

                    if (targetObject.CompareTag("Drop"))
                    {
                        _isRaycastSuccesful = true;
                        return targetObject;
                    }
                }
            }
            
            _isRaycastSuccesful = false;
            return null;
            
            /*_ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(_ray, out _hit))
            {
                GameObject targetObject = _hit.collider.gameObject;
                return targetObject.CompareTag("Drop") ? targetObject : null;
            }

            return null;
            */
        }
    }
}