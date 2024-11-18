using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;
namespace Canons.Trajectories
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PowerValueView : MonoBehaviour, IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new ();

        private TextMeshProUGUI _powerValueText;
        private PowerSliderObserver _powerSliderObserver;

        [Inject]
        public void Construct(PowerSliderObserver powerSliderObserver)
        {
            _powerSliderObserver = powerSliderObserver;
        }

        protected void Awake()
        {
            _powerValueText = GetComponent<TextMeshProUGUI>();
            _powerSliderObserver.Power.Subscribe(delegate (float value)
            {
                _powerValueText.text = $"Power: {value}%";
            }).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
