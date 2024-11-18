using System;
using UniRx;
using UnityEngine;
namespace Inputs
{
    public class KeyboardInput : IDisposable
    {

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Subject<Vector2> _direction = new ();

        public KeyboardInput()
        {
            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    Vector2 direction = new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));

                    _direction.OnNext(direction);
                }).AddTo(_compositeDisposable);
        }

        public IObservable<Vector2> Direction => _direction;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
