using UnityEngine;
using UnityEngine.UI;

namespace NaturalSelectionSimulation
{
    public class SliderHandlerScript : MonoBehaviour
    {
        public Text sliderValue = null;
        public Slider slider = null;
        public string suffix = null;

        // Start is called before the first frame update
        void Start()
        {
            SetSliderValue();
        }

        public void SetSliderValue()
        {
            sliderValue.text = $"{slider.value.ToString("0")} {suffix}";
        }
    }
}
