using UnityEngine;
using System.Collections.Generic;

namespace Arcade.Game
{
    /// <summary>
    /// Generic object pool for efficient instantiation/reuse.
    /// Used for targets, particles, UI elements.
    /// </summary>
    public class ObjectPool<T> where T : Component
    {
        private T prefab;
        private Transform parent;
        private Queue<T> pool = new Queue<T>();
        private int maxSize;

        public ObjectPool(T prefab, int initialSize = 10, int maxSize = 50, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.maxSize = maxSize;

            // Pre-warm pool
            for (int i = 0; i < initialSize; i++)
            {
                T instance = Object.Instantiate(prefab, parent);
                instance.gameObject.SetActive(false);
                pool.Enqueue(instance);
            }
        }

        public T Get()
        {
            T instance;

            if (pool.Count > 0)
            {
                instance = pool.Dequeue();
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Object.Instantiate(prefab, parent);
            }

            return instance;
        }

        public void Return(T instance)
        {
            if (instance == null) return;

            if (pool.Count < maxSize)
            {
                instance.gameObject.SetActive(false);
                pool.Enqueue(instance);
            }
            else
            {
                Object.Destroy(instance.gameObject);
            }
        }

        public void Clear()
        {
            while (pool.Count > 0)
            {
                T instance = pool.Dequeue();
                if (instance != null)
                    Object.Destroy(instance.gameObject);
            }
        }

        public int Count => pool.Count;
    }
}
