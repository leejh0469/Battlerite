using UnityEngine;

namespace CoreSystem.Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private bool _isInitialized = false;

        [Header("Singleton Settings")]
        [Tooltip("체크하면 씬이 변경되어도 파괴되지 않습니다.\n체크 해제하면 씬 변경 시 파괴됩니다.")]
        [SerializeField] protected bool _dontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));

                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();

                        // 컴포넌트 추가
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // T가 MonoSingleton<T>를 상속받은 경우에만 Init 실행
                        if (_instance is MonoSingleton<T> singleton)
                        {
                            singleton.Initialize();
                        }
                    }
                    else
                    {
                        // 씬에 이미 존재했던 경우에도 초기화가 안 되어 있다면 실행
                        if (_instance is MonoSingleton<T> singleton)
                        {
                            singleton.Initialize();
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            if(_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        // 중복 실행 방지를 위한 초기화 진입점
        private void Initialize()
        {
            if (_isInitialized) return;

            _isInitialized = true;
            Init();
        }

        protected virtual void Init() { }
    }
}