using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants

    private const string playerNamePrefKey = "PlayerName";
    
    #endregion

    #region MonoBehaviour Callbacks

    void Start()
    {
        String defaultName = string.Empty;
        TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region public Methods


    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.Log("플레이어 이름이 비어있습니다");
            return;
        }

        PhotonNetwork.NickName = value;
        
        PlayerPrefs.SetString(playerNamePrefKey,value);
    }

    #endregion
}
