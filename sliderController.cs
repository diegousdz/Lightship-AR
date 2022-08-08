using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sliderController : MonoBehaviour
{
   public Slider slider;
   
   public LocalizeMe localizationScript;

   public TMP_Text sliderValue;
   private void Start() {
      updateValue();
   }

   public void updateValue(){
      localizationScript.freaquencyRate = slider.value;
      sliderValue.text = slider.value.ToString();
   }

}
