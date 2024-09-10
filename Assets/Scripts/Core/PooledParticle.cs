using UnityEngine;

public class PooledParticle : MonoBehaviour
{
    private ParticlePoolSystem _pool;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetPool(ParticlePoolSystem pool)
    {
        this._pool = pool;
    }

    public void Play(Vector3 position, Color color)
    {
        transform.position = position;

        var mainModule = _particleSystem.main;
        mainModule.startColor = color;

        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _pool.ReturnToPool(this);
    }
}
