using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class UserRegistration : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public string user_id;
        public string user_pwd;
        public string user_name;
        public string user_phone;
        public string user_email;
    }

    public void Registeruser(string userId, string userPwd, string userName, string userPhone, string userEmail)
    {
        UserData newUser = new UserData
        {
            user_id = userId,
            user_pwd = userPwd,
            user_name = userName,
            user_phone = userPhone,
            user_email = userEmail
        };

        string jsonData = JsonUtility.ToJson(newUser);

        StartCoroutine(PostRequest("http://localhost:3000/api/users/register", jsonData));
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type","application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Respone:" + request.downloadHandler.text);
        }
    }
    
}
