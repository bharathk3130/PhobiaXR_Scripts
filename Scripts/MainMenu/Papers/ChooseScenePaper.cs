using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseScenePaper : MonoBehaviour
{
    [SerializeField] Button _liftButton;
    [SerializeField] Button _heightsButton;
    [SerializeField] Button _darknessButton;
    [SerializeField] GameObject _buttonsDisabledText;

    [SerializeField] Color _disabledCol;

    Image _liftButtonImage;
    Image _heightsButtonImage;
    Image _darknessButtonImage;
    
    TextMeshProUGUI _liftButtonText;
    TextMeshProUGUI _heightsButtonText;
    TextMeshProUGUI _darknessButtonText;

    Color _originalButtonCol;
    Color _originalButtonTextCol;

    bool _isActive;
    
    void Awake()
    {
        InitializeReferences();
        
        _originalButtonCol = _liftButtonImage.color;
        _originalButtonTextCol = _liftButtonText.color;
        
        if (!_isActive)
        {
            DeactiveteButtons(); // This runs after ActivateButtons() gets run by HeartRatePaper.Reset(). So we're avoiding that here
        }
    }

    void InitializeReferences()
    {
        _liftButtonText = _liftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _heightsButtonText = _heightsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _darknessButtonText = _darknessButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        _liftButtonImage = _liftButton.GetComponent<Image>();
        _heightsButtonImage = _heightsButton.GetComponent<Image>();
        _darknessButtonImage = _darknessButton.GetComponent<Image>();
    }

    public void ActivateButtons()
    {
        if (_liftButtonText == null)
        {
            InitializeReferences();
        }
        
        _isActive = true;
        _liftButton.interactable = true;
        _heightsButton.interactable = true;
        _darknessButton.interactable = true;
        
        _liftButtonImage.color = _originalButtonCol;
        _heightsButtonImage.color = _originalButtonCol;
        _darknessButtonImage.color = _originalButtonCol;
        
        _liftButtonText.color = _originalButtonTextCol;
        _heightsButtonText.color = _originalButtonTextCol;
        _darknessButtonText.color = _originalButtonTextCol;
        
        _buttonsDisabledText.SetActive(false);
    }

    public void DeactiveteButtons()
    {
        _isActive = false;
        _liftButtonImage.color = _disabledCol;
        _heightsButtonImage.color = _disabledCol;
        _darknessButtonImage.color = _disabledCol;
        
        _liftButtonText.color = _disabledCol;
        _heightsButtonText.color = _disabledCol;
        _darknessButtonText.color = _disabledCol;
        
        _liftButton.interactable = false;
        _heightsButton.interactable = false;
        _darknessButton.interactable = false;
        
        _buttonsDisabledText.SetActive(true);
    }
}
