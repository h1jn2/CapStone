using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance = null;
    
    private const string DB_TABLE_USER_LOGIN = "user";
    public static bool isLogin = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        
        Debug.LogError("실행성공");
    }

    private void view()
    {
        XmlNodeList userIdList = DBConnector.Select(DB_TABLE_USER_LOGIN);
        foreach (XmlNode node in userIdList)
        {
            Debug.Log(node.ParseXmlNode<string>("user_id"));
            Debug.Log(node.ParseXmlNode<string>("user_pwd"));
        }

        Debug.Log("=================================================");
    }

    public bool CheckID(string inputID)
    {
        bool check = false;
        XmlNodeList userIdList = DBConnector.Select(DB_TABLE_USER_LOGIN);
        foreach (XmlNode node in userIdList)
        {
            if (node.ParseXmlNode<string>("user_id") == inputID)
            {
                check = true;
            }
        }
        return check;
    }

    public bool CheckPwd(string InputID, string InputPwd)
    {
        bool check = false;

        XmlNodeList userPwdList = DBConnector.Select(DB_TABLE_USER_LOGIN, "user_pwd", "where user_id = '" +InputID + "'");

        if (userPwdList.Count != 0)
        {
            foreach (XmlNode node in userPwdList)
            {
                if (node.ParseXmlNode<string>("user_pwd") == InputPwd)
                {
                    check = true;
                }
            }    
        }

        if (check)
        {
            isLogin = true;
        }
        return check;
    }
}
