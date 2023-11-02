using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Managers
{
    public class ObjectPoolManager: SingletonMono<ObjectPoolManager>
    {
        [SerializeField] private List<string> dropPrefabNames;
        [SerializeField] private Transform poolParent;
        private GameObject[] _dropPrefabs;
        private Queue<GameObject> _poolDictionary;
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        
        private void InitializePools()
        {
            CreateObjectPool();
        }
        
        private void CreateObjectPool()
        {
            _poolDictionary = new Queue<GameObject>();
        }
        
        public void DestroyDrop(GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
            objectToReturn.transform.SetParent(poolParent);
            _poolDictionary.Enqueue(objectToReturn);
        }
        private int GetPrefabIndex(DropType type)
        {
            for (int i = 0; i < _dropPrefabs.Length; i++)
            {
                if (_dropPrefabs[i].GetComponent<Drop>().dropType == type)
                {
                    return i;
                }
            }
            return 0;
        }
        
        public GameObject InstantiateDrop(Vector3 position, Quaternion rotation, int typeToInt = -1, Transform parent = null)
        {
            DropType type;
            if (typeToInt == -1)
            {
                type = (DropType)Random.Range(0, _dropPrefabs.Length);
            }
            else
            {
                type = (DropType)typeToInt;
            }

            GameObject objectToSpawn;
            
            if (_poolDictionary.Count == 0)
            {
                objectToSpawn = Instantiate(_dropPrefabs[GetPrefabIndex(type)], position, rotation);
                objectToSpawn.SetActive(false);
                objectToSpawn.transform.parent = poolParent;
            }
            else
            {
                objectToSpawn = _poolDictionary.Dequeue();
                objectToSpawn.GetComponent<Drop>().dropType = type;
                
                objectToSpawn.GetComponentInChildren<SpriteFromAtlas>().GenerateSprite(dropPrefabNames[(int)type]);
            }

            if (parent != null)
            {
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.transform.localPosition = Vector3.zero;
            }
            
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.localScale = Vector3.one;
            
            return objectToSpawn;
        }
        
        private void SubscribeEvents() => CoreGameSignals.Instance.OnDropListTaken += SetDropPrefabs;
        private void UnSubscribeEvents() => CoreGameSignals.Instance.OnDropListTaken -= SetDropPrefabs;
        private void SetDropPrefabs(GameObject[] prefabs)
        {
            _dropPrefabs = prefabs;
            InitializePools();
        }
    }
}