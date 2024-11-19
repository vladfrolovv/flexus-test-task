using System;
using Cameras;
using Canons.CannonBalls;
using DG.Tweening;
using Inputs;
using UniRx;
using UnityEngine;
namespace Canons
{
    public class CannonPresenter : IDisposable
    {

        private readonly CannonBarrelView _cannonBarrelView;
        private readonly CannonConfig _cannonConfig;
        private readonly CannonView _cannonView;
        private readonly CameraView _cameraView;
        private readonly CameraShakeEffect _cameraShakeEffect;
        private readonly CannonballFactory _cannonballFactory;
        private readonly CannonballLaunchPoint _cannonballLaunchPoint;

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly Vector3 _standardCanonBarrelPosition;

        private readonly float _distanceFromCamera;
        private readonly float _angleFromCamera;
        private float _currentPitch;

        private DateTime _lastShotTime;

        public CannonPresenter(CanonInput canonInput, KeyboardInput keyboardInput, CannonView cannonView, CannonBarrelView cannonBarrelView, CannonConfig cannonConfig,
                              CameraView cameraView, CameraShakeEffect cameraShakeEffect, CannonballFactory cannonballFactory, CannonballLaunchPoint cannonballLaunchPoint)
        {
            _cannonBarrelView = cannonBarrelView;
            _cannonConfig = cannonConfig;
            _cannonView = cannonView;
            _cameraView = cameraView;
            _cameraShakeEffect = cameraShakeEffect;
            _cannonballFactory = cannonballFactory;
            _cannonballLaunchPoint = cannonballLaunchPoint;

            _standardCanonBarrelPosition = _cannonBarrelView.LocalPosition;
            _distanceFromCamera = Vector2.Distance(
                new Vector2(_cannonView.Position.x, _cannonView.Position.z),
                new Vector2(_cameraView.Position.x, _cameraView.Position.z));

            _angleFromCamera = Mathf.Asin((_cannonView.Position.z - _cameraView.Position.z) / _distanceFromCamera) * Mathf.Rad2Deg;

            CanonRotation();

            keyboardInput.PitchDirection.Subscribe(CanonBarrelElevation).AddTo(_compositeDisposable);
            keyboardInput.YawDirection.Subscribe(CanonRotation).AddTo(_compositeDisposable);
            canonInput.ShotInput.Subscribe(delegate (bool b)
            {
                if ((DateTime.Now - _lastShotTime).TotalSeconds < _cannonConfig.ShotCooldown)
                {
                    return;
                }
                Shot();
                AnimateShot();

                _lastShotTime = DateTime.Now;
            }).AddTo(_compositeDisposable);
        }

        private void CanonRotation(Vector2Int direction)
        {
            CanonRotation();
        }

        private void CanonBarrelElevation(Vector2Int direction)
        {
            float deltaPitch = -direction.y * _cannonConfig.ElevationSpeed;

            _currentPitch = Mathf.Clamp(_currentPitch - deltaPitch, _cannonConfig.MinElevation, _cannonConfig.MaxElevation);
            _cannonBarrelView.LocalRotation = Quaternion.Euler(-_currentPitch, 0, 0);
        }

        private void CanonRotation()
        {
            float yaw = _cameraView.Rotation.eulerAngles.y + _angleFromCamera;

            Vector3 newPosition = new (
                _cameraView.Position.x + Mathf.Sin(yaw * Mathf.Deg2Rad) * _distanceFromCamera,
                _cannonView.Position.y,
                _cameraView.Position.z + Mathf.Cos(yaw * Mathf.Deg2Rad) * _distanceFromCamera
            );

            _cannonView.Position = newPosition;
            _cannonView.Rotation = _cameraView.Rotation;
        }

        private void Shot()
        {
            Cannonball cannonball = _cannonballFactory.Create(new CannonballInfo());
            cannonball.transform.position = _cannonballLaunchPoint.Position;
            cannonball.Launch();
        }

        private void AnimateShot()
        {
            _cameraShakeEffect.Shake();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_cannonBarrelView.transform.DOLocalMove(_standardCanonBarrelPosition - _cannonballLaunchPoint.Direction * 0.25f, 0.08f).SetEase(Ease.InBack));
            sequence.AppendInterval(0.05f);
            sequence.Append(_cannonBarrelView.transform.DOLocalMove(_standardCanonBarrelPosition, 0.05f).SetEase(Ease.OutCirc));
            sequence.Play();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
