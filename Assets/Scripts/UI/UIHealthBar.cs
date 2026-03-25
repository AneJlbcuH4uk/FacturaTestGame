using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Color lowLifeColor;
    [SerializeField] private Color mediumLifeColor;
    [SerializeField] private Color highLifeColor;
    [SerializeField] private Image fillImage;


    public void SetValue(float normalized)
    {
        slider.value = normalized;
        fillImage.color = SelectColor(normalized);
    }

    private Color SelectColor(float val) => val switch
    {
        > 0.6f => highLifeColor,
        > 0.3f => mediumLifeColor,
        > 0.0f => lowLifeColor,
        _ => highLifeColor,
    };
   
}