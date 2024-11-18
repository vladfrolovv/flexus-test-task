using System;
using Cameras;
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

        private readonly float _standardCanonBarrelZ;

        private readonly CompositeDisposable _compositeDisposable = new();

        private float _currentPitch;

        public CanonPresenter(CanonInput canonInput, KeyboardInput keyboardInput, CanonView canonView, CanonBarrelView canonBarrelView, CanonConfig canonConfig,
                              CameraView cameraView)
        {
            _canonBarrelView = canonBarrelView;
            _canonConfig = canonConfig;
            _canonView = canonView;
            _cameraView = cameraView;

            _standardCanonBarrelZ = _canonBarrelView.transform.localPosition.z;

            keyboardInput.Direction.Subscribe(delegate (Vector2Int direction)
            {
                CanonRotation(direction);
                CanonBarrelElevation(direction);
            }).AddTo(_compositeDisposable);

            canonInput.ShotInput.Subscribe(delegate (bool b)
            {
                Shot();
                AnimateShot();
            }).AddTo(_compositeDisposable);
        }

        private void CanonBarrelElevation(Vector2Int direction)
        {
            float deltaPitch = -direction.y * _canonConfig.ElevationSpeed;

            _currentPitch = Mathf.Clamp(_currentPitch - deltaPitch, _canonConfig.MinElevation, _canonConfig.MaxElevation);
            _canonBarrelView.Rotation = Quaternion.Euler(-_currentPitch, 0, 0);
        }

        private void CanonRotation(Vector2Int direction)
        {
            float deltaYaw = direction.x * cameraPanConfig.PanSpeed;
            _currentYaw = Mathf.Clamp(_currentYaw + deltaYaw, -cameraPanConfig.MaxYaw, cameraPanConfig.MaxYaw);

            cameraView.Rotation = Quaternion.Euler(0, _currentYaw, 0);
            cameraView.Rotation *= Quaternion.Euler(Vector3.up * direction.x * cameraPanConfig.PanSpeed);
        }

        private void Shot()
        {
            throw new NotImplementedException();
        }

        private void AnimateShot()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canonBarrelView.transform.DOLocalMoveZ(_standardCanonBarrelZ - 0.25f, 0.08f).SetEase(Ease.InBack));
            sequence.AppendInterval(0.05f);
            sequence.Append(_canonBarrelView.transform.DOLocalMoveZ(_standardCanonBarrelZ, 0.05f).SetEase(Ease.OutCirc));
            sequence.Play();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
