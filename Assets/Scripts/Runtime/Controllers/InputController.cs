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
    public class InputController
    {
        private bool _isAvailableTouch;
        private InputData _inputData;
        
        private SwipeDirection _swipeDirection;
        private GameObject _targetDropObject;
        
        private Vector2 _firstTouchPosition;
        private Ray _ray;
        private RaycastHit _hit;
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

            if (Input.GetMouseButtonUp(0))
            {
                ProcessTouchEnd();
            }
            
            InputSignals.Instance.onInputTaken?.Invoke(new MatchInfoParams(_swipeDirection, _targetDropObject));
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
                if (!IsSwipeDistanceFit(Math.Abs(verticalSwipeDelta)))
                    return;
                    
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
            else if (horizontalSwipeDelta > verticalSwipeDelta)
            {
                if (!IsSwipeDistanceFit(Math.Abs(horizontalSwipeDelta)))
                    return;
                    
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
            else
            {
                //Debug.LogWarning("There is no swipe");
                return;
            }
        }
        private GameObject GetRaycastTarget()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                GameObject targetObject= result.gameObject;
                return targetObject.CompareTag("Drop") ? targetObject : null;
            }

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
        private bool IsSwipeDistanceFit(float distance)
        {
            if (distance > _inputData.VerticalSwipeDistance
                && distance < _inputData.MaxVerticalSwipeDistance)
            {
                return true;
            }

            return false;
        }
    }
}