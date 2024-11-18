using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Canons.Trajectories
{
    [RequireComponent(typeof(Slider))]
    public class PowerSliderObserver : MonoBehaviour
    {

        private readonly ReactiveProperty<float> _power = new ();

        private Slider _powerSlider;

        protected void Awake()
        {
            _powerSlider = GetComponent<Slider>();
            _power.Value = _powerSlider.value;

            _powerSlider.onValueChanged.AddListener(delegate(float value)
            {
                _power.Value = value;
            });
        }

        public IReadOnlyReactiveProperty<float> Power => _power;

    }
}
