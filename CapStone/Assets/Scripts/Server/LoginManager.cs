using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LoginManager : MonoBehaviour
{
    private const string DB_TABLE_USER_LOGIN = "user";
    void Start()
    {
        view();
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
    
}
