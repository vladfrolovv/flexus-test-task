using System;
using UniRx;
using UnityEngine;
namespace Inputs
{
    public class MouseInput : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Subject<Vector2> _mousePosition = new ();

        public MouseInput()
        {
            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    _mousePosition.OnNext(Input.mousePosition);
                }).AddTo(_compositeDisposable);
        }

        public IObservable<Vector2> MousePosition => _mousePosition;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
