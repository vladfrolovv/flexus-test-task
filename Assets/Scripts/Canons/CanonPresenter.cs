﻿using System;
using Cameras;
using Canons.CannonBalls;
using DG.Tweening;
using Inputs;
using UniRx;
using UnityEngine;
namespace Canons
{
    public class CanonPresenter : IDisposable
    {

        private readonly CanonBarrelView _canonBarrelView;
        private readonly CanonConfig _canonConfig;
        private readonly CanonView _canonView;
        private readonly CameraView _cameraView;
        private readonly CameraShakeEffect _cameraShakeEffect;
        private readonly CannonballFactory _cannonballFactory;
        private readonly CannonballLaunchPoint _cannonballLaunchPoint;

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly float _distanceFromCamera;
        private readonly float _angleFromCamera;
        private float _currentPitch;

        private Vector3 _standardCanonBarrelPosition;

        public CanonPresenter(CanonInput canonInput, KeyboardInput keyboardInput, CanonView canonView, CanonBarrelView canonBarrelView, CanonConfig canonConfig,
                              CameraView cameraView, CameraShakeEffect cameraShakeEffect, CannonballFactory cannonballFactory, CannonballLaunchPoint cannonballLaunchPoint)
        {
            _canonBarrelView = canonBarrelView;
            _canonConfig = canonConfig;
            _canonView = canonView;
            _cameraView = cameraView;
            _cameraShakeEffect = cameraShakeEffect;
            _cannonballFactory = cannonballFactory;
            _cannonballLaunchPoint = cannonballLaunchPoint;

            _standardCanonBarrelPosition = _canonBarrelView.LocalPosition;
            _distanceFromCamera = Vector2.Distance(
                new Vector2(_canonView.Position.x, _canonView.Position.z),
                new Vector2(_cameraView.Position.x, _cameraView.Position.z));

            _angleFromCamera = Mathf.Asin((_canonView.Position.z - _cameraView.Position.z) / _distanceFromCamera) * Mathf.Rad2Deg;

            CanonRotation();

            keyboardInput.PitchDirection.Subscribe(CanonBarrelElevation).AddTo(_compositeDisposable);
            keyboardInput.YawDirection.Subscribe(CanonRotation).AddTo(_compositeDisposable);
            canonInput.ShotInput.Subscribe(delegate (bool b)
            {
                Shot();
                AnimateShot();
            }).AddTo(_compositeDisposable);
        }

        private void CanonRotation(Vector2Int direction)
        {
            CanonRotation();
        }

        private void CanonBarrelElevation(Vector2Int direction)
        {
            float deltaPitch = -direction.y * _canonConfig.ElevationSpeed;

            _currentPitch = Mathf.Clamp(_currentPitch - deltaPitch, _canonConfig.MinElevation, _canonConfig.MaxElevation);
            _canonBarrelView.LocalRotation = Quaternion.Euler(-_currentPitch, 0, 0);
        }

        private void CanonRotation()
        {
            float yaw = _cameraView.Rotation.eulerAngles.y + _angleFromCamera;

            Vector3 newPosition = new (
                _cameraView.Position.x + Mathf.Sin(yaw * Mathf.Deg2Rad) * _distanceFromCamera,
                _canonView.Position.y,
                _cameraView.Position.z + Mathf.Cos(yaw * Mathf.Deg2Rad) * _distanceFromCamera
            );

            _canonView.Position = newPosition;
            _canonView.Rotation = _cameraView.Rotation;
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
            sequence.Append(_canonBarrelView.transform.DOLocalMove(_standardCanonBarrelPosition - _cannonballLaunchPoint.Direction * 0.25f, 0.08f).SetEase(Ease.InBack));
            sequence.AppendInterval(0.05f);
            sequence.Append(_canonBarrelView.transform.DOLocalMove(_standardCanonBarrelPosition, 0.05f).SetEase(Ease.OutCirc));
            sequence.Play();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
