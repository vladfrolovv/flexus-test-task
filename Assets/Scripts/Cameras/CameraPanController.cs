﻿using System;
using Inputs;
using UniRx;
using UnityEngine;
namespace Cameras
{
    public class CameraPanController : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();

        private float _currentYaw;

        public CameraPanController(KeyboardInput keyboardInput, CameraView cameraView, CameraPanConfig cameraPanConfig)
        {
            keyboardInput
                .YawDirection
                .Subscribe(delegate (Vector2Int direction)
                {
                    float deltaYaw = direction.x * cameraPanConfig.PanSpeed;
                    _currentYaw = Mathf.Clamp(_currentYaw + deltaYaw, -cameraPanConfig.MaxYaw, cameraPanConfig.MaxYaw);

                    cameraView.Rotation = Quaternion.Euler(0, _currentYaw, 0);
                    cameraView.Rotation *= Quaternion.Euler(Vector3.up * direction.x * cameraPanConfig.PanSpeed);
                }).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
