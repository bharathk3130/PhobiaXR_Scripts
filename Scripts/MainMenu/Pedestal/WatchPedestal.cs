using System.Collections;
using KBCore.Refs;
using UnityEngine;

public class WatchPedestal : ValidatedMonoBehaviour
{
    [SerializeField, Scene] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField, Scene] WatchEquipEffect _watchEquipEffect;
    [SerializeField, Child] GroundCutout _groundCutout;
    [SerializeField] ButtonClickDemo _buttonClickDemo;
    [SerializeField] Animator _pedestalAnim;
    [SerializeField] GameObject _canvasGO;
    [SerializeField] Animator _instructionAnim;
    [SerializeField] GameObject _realWatch;

    static bool s_started;
    static bool s_isComplete;

    static bool s_completedThisSession;

    static readonly int _goBackHash = Animator.StringToHash("GoBack");
    static readonly int _fadeOutHash = Animator.StringToHash("FadeOut");

    void Start()
    {
        _pedestalAnim.gameObject.SetActive(false);
        _canvasGO.SetActive(false);

        s_isComplete = PlayerPrefsManager.DataExists(GetType().Name);
        if (s_isComplete)
        {
            _realWatch.SetActive(true);
        } else
        {
            _realWatch.SetActive(false);
        }
    }

    public void Begin()
    {
        if (s_isComplete)
        {
            s_completedThisSession = true;
            return;
        }
        
        if (s_started) return;

        if (!_buttonClickDemo.IsCompleted)
        {
            // Wait for button click demo to complete before starting this - don't want to clutter the screen with multiple demos
            _buttonClickDemo.OnComplete += Begin;
            return;
        }

        s_started = true;
        StartCoroutine(StartProcess());
    }

    IEnumerator StartProcess()
    {
        _groundCutout.CreateOpening();
        yield return new WaitForSeconds(1);
        _pedestalAnim.gameObject.SetActive(true);
        _mainMenuSFXManager.PlayPedestalRisingSFX(_pedestalAnim.transform.position);

        Invoke(nameof(EnableInstructionIfNotGrabbed), 7);
    }

    void EnableInstructionIfNotGrabbed()
    {
        if (!s_isComplete)
        {
            _canvasGO.SetActive(true);
        }
    }

    public void OnWatchGrabbed()
    {
        s_isComplete = true;
        s_completedThisSession = true;
        _realWatch.SetActive(true);
        _watchEquipEffect.PlayEffect();
        PlayerPrefsManager.SaveData(GetType().Name, 1);

        if (_canvasGO.activeSelf)
        {
            _instructionAnim.SetTrigger(_fadeOutHash);
            Invoke(nameof(DisableCanvas), 1);
        }

        StartCoroutine(RetractPedestalAndEnableButtons());
    }

    void DisableCanvas() => _canvasGO.SetActive(false);

    IEnumerator RetractPedestalAndEnableButtons()
    {
        yield return new WaitForSeconds(2);
        _pedestalAnim.SetTrigger(_goBackHash);
        _mainMenuSFXManager.PlayPedestalRetreatingSFX(_pedestalAnim.transform.position);

        yield return new WaitForSeconds(2.5f);
        _groundCutout.CloseOpening();

        yield return new WaitForSeconds(1.5f);
    }

    [ContextMenu("Clear Pedestal Data")]
    void ClearPedestalData() => PlayerPrefsManager.ClearData(GetType().Name);
}