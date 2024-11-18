using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Cameras
{
    public class CameraShakeEffect : MonoBehaviour
    {

        [SerializeField] private Transform _cameraTransform;

        [Header("Shake Settings")]
        [SerializeField] private Vector3 _maximumTranslationShake = Vector3.one;
        [SerializeField] private float _standardStress = 1f;
        [SerializeField] private float _frequency = 25;
        [SerializeField] private float _traumaExponent = 2;
        [SerializeField] private float _recoverySpeed = 1f;

        private float _trauma;
        private float _seed;

        private IDisposable _shakeDisposable;

        private void Awake()
        {
            _seed = Random.value;
        }

        public void Shake()
        {
            _trauma = Mathf.Clamp01(_trauma + _standardStress);
            _shakeDisposable?.Dispose();
            _shakeDisposable = Observable.EveryUpdate().Subscribe(delegate
            {
                float shake = Mathf.Pow(_trauma, _traumaExponent);

                if (shake <= 0)
                {
                    _shakeDisposable?.Dispose();
                }

                _cameraTransform.localPosition = new Vector3(
                    _maximumTranslationShake.x * (Mathf.PerlinNoise(_seed * 1, Time.time * _frequency) * 2 - 1),
                    _maximumTranslationShake.y * (Mathf.PerlinNoise(_seed + 1, Time.time * _frequency) * 2 - 1),
                    _maximumTranslationShake.z * (Mathf.PerlinNoise(_seed - 1, Time.time * _frequency) * 2 - 1)
                ) * shake;

                _trauma = Mathf.Clamp01(_trauma - _recoverySpeed * Time.deltaTime);
            });
        }
    }
}
