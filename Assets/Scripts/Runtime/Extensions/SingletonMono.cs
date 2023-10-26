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
                //DontDestroyOnLoad(instance);
            }
            else if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        
        public string Classname
        {
            get
            {
                return GetType().Name;
            }
        }

        public static string MethodName
        {
            get
            {
                return  new StackTrace().GetFrame(1).GetMethod().Name;
            }
        }
    }
}