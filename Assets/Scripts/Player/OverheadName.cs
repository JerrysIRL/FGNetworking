using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class OverheadName : NetworkBehaviour
{
    [SerializeField] private Name userName;

    private Text _overheadText; 
    private void Start()
    {
        _overheadText = GetComponent<Text>();
        _overheadText.text = userName.userNameNetwork.Value.ToString();
        userName.userNameNetwork.OnValueChanged += ChangeUI;
    }

    private void ChangeUI(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        _overheadText.text = newValue.ToString();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        userName.userNameNetwork.OnValueChanged -= ChangeUI;
    }
}