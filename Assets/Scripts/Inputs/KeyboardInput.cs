using System;
using UniRx;
using UnityEngine;
namespace Inputs
{
    public class KeyboardInput : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Subject<Vector2Int> _direction = new ();

        public KeyboardInput()
        {
            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    {
                        _direction.OnNext(Vector2Int.left);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    {
                        _direction.OnNext(Vector2Int.right);
                    }
                    else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    {
                        _direction.OnNext(Vector2Int.up);
                    }
                    else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                    {
                        _direction.OnNext(Vector2Int.down);
                    }
                }).AddTo(_compositeDisposable);
        }

        public IObservable<Vector2Int> Direction => _direction;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
