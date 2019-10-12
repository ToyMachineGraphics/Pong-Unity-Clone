using UnityEngine;

namespace PongClone
{
    public class DeathVFX : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            _particleSystem.Play(true);
        }

        private void OnParticleSystemStopped()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}