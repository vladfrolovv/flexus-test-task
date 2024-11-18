using System;
using UniRx;
using UnityEngine;
namespace Inputs
{
    public class CanonInput : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Subject<bool> _shotInput = new();

        public CanonInput(InputBlocker inputBlocker)
        {
            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    if (inputBlocker.IsPointerOverUI()) return;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _shotInput.OnNext(true);
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        _shotInput.OnNext(true);
                    }
                }).AddTo(_compositeDisposable);
        }

        public IObservable<bool> ShotInput => _shotInput;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }



    }
}
