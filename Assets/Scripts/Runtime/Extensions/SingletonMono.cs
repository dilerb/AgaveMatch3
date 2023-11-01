using System.Diagnostics;
using UnityEngine;

namespace Runtime.Extensions
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;
        
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                //_instance = FindObjectOfType(typeof(T)) as T;
            }
            else if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}