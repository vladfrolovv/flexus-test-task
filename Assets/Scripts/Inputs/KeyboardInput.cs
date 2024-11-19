using System;
using UniRx;
using UnityEngine;
namespace Inputs
{
    public class KeyboardInput : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly Subject<Vector2Int> _direction = new ();
        private readonly Subject<Vector2Int> _pitchDirection = new ();
        private readonly Subject<Vector2Int> _yawDirection = new ();

        public KeyboardInput()
        {
            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    {
                        _direction.OnNext(Vector2Int.left);
                        _yawDirection.OnNext(Vector2Int.left);
                    }

                    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    {
                        _direction.OnNext(Vector2Int.right);
                        _yawDirection.OnNext(Vector2Int.right);
                    }

                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    {
                        _direction.OnNext(Vector2Int.up);
                        _pitchDirection.OnNext(Vector2Int.up);
                    }

                    if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                    {
                        _direction.OnNext(Vector2Int.down);
                        _pitchDirection.OnNext(Vector2Int.down);
                    }
                }).AddTo(_compositeDisposable);
        }

        public IObservable<Vector2Int> Direction => _direction;
        public IObservable<Vector2Int> PitchDirection => _pitchDirection;
        public IObservable<Vector2Int> YawDirection => _yawDirection;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
