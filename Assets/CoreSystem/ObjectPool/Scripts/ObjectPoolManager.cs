using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using CoreSystem;
using System;
using UnityEngine.Pool;

namespace CoreSystem
{ 
    [System.Serializable] 
    public class PoolInfo
    {
        public string key;
        public GameObject prefab;
        public int initCount;
        [HideInInspector] public Transform container;
    }

    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        [SerializeField]
        private List<PoolInfo> poolList;

        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private Dictionary<string, PoolInfo> _poolInfoDictionary;

        private Transform _rootContainerTransform;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            // 다른 씬으로 넘어가도 유지하고 싶다면 주석 해제
            // DontDestroyOnLoad(gameObject); 

            InitializePool();
        }

        private void InitializePool()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>(StringComparer.OrdinalIgnoreCase);
            _poolInfoDictionary = new Dictionary<string, PoolInfo>(StringComparer.OrdinalIgnoreCase);

            GameObject rootContainer = new GameObject("@ObjectPool_Root");
            rootContainer.transform.SetParent(this.transform);
            _rootContainerTransform = rootContainer.transform;

            foreach (var info in poolList)
            {
                // 이미 존재하는 키라면 스킵
                if (_poolDictionary.ContainsKey(info.key)) continue;

                // RegisterPool 로직을 재활용하여 초기화
                RegisterPoolInternal(info, _rootContainerTransform);
            }
        }

        private GameObject CreateNewObject(PoolInfo info)
        {
            GameObject obj = Instantiate(info.prefab, info.container);
            obj.name = info.key;
            obj.SetActive(false);
            return obj;
        }

        private void RegisterPoolInternal(PoolInfo info, Transform root)
        {
            if (!_poolDictionary.ContainsKey(info.key))
            {
                _poolDictionary.Add(info.key, new Queue<GameObject>());
                _poolInfoDictionary.Add(info.key, info);
            }

            // 하위 컨테이너 생성
            GameObject subContainer = new GameObject($"Pool_{info.key}");
            subContainer.transform.SetParent(root);
            info.container = subContainer.transform;

            // 초기 생성
            for (int i = 0; i < info.initCount; i++)
            {
                GameObject obj = CreateNewObject(info);
                _poolDictionary[info.key].Enqueue(obj);
            }
        }

        public void RegisterPool(string key, GameObject prefab, int initialCount = 10)
        {
            // 1. 딕셔너리가 초기화되지 않았으면 초기화 (Awake 전 호출 대비)
            if (_poolDictionary == null)
            {
                _poolDictionary = new Dictionary<string, Queue<GameObject>>(StringComparer.OrdinalIgnoreCase);
                _poolInfoDictionary = new Dictionary<string, PoolInfo>(StringComparer.OrdinalIgnoreCase);
            }

            // 2. 루트 컨테이너가 없으면 생성
            if (_rootContainerTransform == null)
            {
                GameObject rootContainer = new GameObject("@ObjectPool_Root");
                rootContainer.transform.SetParent(this.transform);
                _rootContainerTransform = rootContainer.transform;
            }

            // 3. 중복 키 방지
            if (_poolDictionary.ContainsKey(key))
            {
                // Debug.LogWarning($"[ObjectPool] Pool `{key}` already exists.");
                return;
            }

            // 4. 새로운 PoolInfo 생성 (인스펙터 없이 코드로 생성)
            PoolInfo newInfo = new PoolInfo
            {
                key = key,
                prefab = prefab,
                initCount = initialCount
            };

            // 5. 등록 및 생성 실행
            RegisterPoolInternal(newInfo, _rootContainerTransform);

            Debug.Log($"[ObjectPool] Dynamic Pool Registered: {key}");
        }

        public GameObject Spawn(string key)
        {
            return Spawn(key, Vector3.zero, Quaternion.identity);
        }

        public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"[ObjectPool] Key '{key}' not found via Inspector!");
                return null;
            }

            GameObject obj;

            if (_poolDictionary[key].Count > 0)
            {
                obj = _poolDictionary[key].Dequeue();
            }
            else
            {
                obj = CreateNewObject(_poolInfoDictionary[key]);
            }

            if (!obj.TryGetComponent(out PoolableObject poolObject))
            {
                poolObject = obj.AddComponent<PoolableObject>();
            }

            poolObject.SetKey(key);

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            Debug.Log($"{key} object spawn");

            return obj;
        }

        public void Despawn(string key, GameObject obj)
        {
            if (!_poolInfoDictionary.ContainsKey(key))
            {
                Debug.LogError($"[ObjectPool] Invalid Key '{key}' for Despawn!");
                Destroy(obj); // 관리 대상 아니면 그냥 파괴
                return;
            }

            obj.SetActive(false);

            obj.transform.SetParent(_poolInfoDictionary[key].container);

            // 4. 대기열 반납
            _poolDictionary[key].Enqueue(obj);

            Debug.Log($"{key} object despawn");
        }

        public void Despawn(GameObject obj)
        {
            PoolableObject poolObj = obj.GetComponent<PoolableObject>();

            // 2. 명찰이 없으면? 이건 우리 풀에서 관리하는 애가 아닙니다.
            if (poolObj == null)
            {
                Debug.LogWarning($"[ObjectPool] '{obj.name}'은 풀 관리 대상이 아닙니다. 그냥 파괴합니다.");
                Destroy(obj);
                return;
            }

            // 3. 명찰에 적힌 키를 보고 원래 있던 곳으로 돌려보냅니다.
            Despawn(poolObj.PoolKey, obj);
        }
    }
}
