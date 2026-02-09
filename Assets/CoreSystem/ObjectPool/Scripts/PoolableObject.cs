using UnityEngine;

namespace CoreSystem
{
    public class PoolableObject : MonoBehaviour
    {
        public string PoolKey { get; private set; }

        public void SetKey(string key)
        {
            PoolKey = key;
        }

        public void ReturnToPool()
        {
            ObjectPoolManager.Instance.Despawn(this.gameObject);
        }
    }

}
