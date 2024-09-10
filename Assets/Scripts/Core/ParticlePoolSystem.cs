using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolSystem : MonoBehaviour
{

    [SerializeField] private PooledParticle _particlePrefab;
    [SerializeField] private int _poolSize = 30;

    private Queue<PooledParticle> _particlePool = new Queue<PooledParticle>();
    public static ParticlePoolSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewParticle();
        }
    }

    private PooledParticle CreateNewParticle()
    {
        PooledParticle pooledParticle = Instantiate(_particlePrefab, transform);

        pooledParticle.SetPool(this);
        pooledParticle.gameObject.SetActive(false);

        _particlePool.Enqueue(pooledParticle);

        return pooledParticle;
    }

    public void PlayParticle(Vector3 position, Color color)
    {
        if (_particlePool.Count == 0)
        {
            CreateNewParticle();
        }

        PooledParticle pooledParticle = _particlePool.Dequeue();
        pooledParticle.gameObject.SetActive(true);
        pooledParticle.Play(position, color);
    }

    public void ReturnToPool(PooledParticle pooledParticle)
    {
        pooledParticle.gameObject.SetActive(false);
        _particlePool.Enqueue(pooledParticle);
    }
}
