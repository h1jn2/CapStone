using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebLoginManager : MonoBehaviour
{
    private string apiUrl = "http://221.166.156.80:3000/api/users"; // 로그인 API URL
    
    [SerializeField]
    private LoginMgr LoginMgr;

    public void Login(string userId, string userPwd)
    {
        Debug.Log("Login method called"); // 디버그 로그 추가
        StartCoroutine(LoginCoroutine(userId, userPwd));
    }

    public void Logout()
    {
        Debug.Log("Logout method called"); // 디버그 로그 추가
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LoginCoroutine(string userId, string userPwd)
    {
        Debug.Log("LoginCoroutine started"); // 디버그 로그 추가

        // JSON 형식의 로그인 데이터
        var loginData = new LoginData { user_id = userId, user_pwd = userPwd };
        var jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Login JSON: " + jsonData); // 디버그 로그 추가

        // POST 요청 설정
        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        Debug.Log("Request sent"); // 디버그 로그 추가

        // 응답 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            LoginMgr.ShowLoginWarning("로그인 실패: 서버 오류가 발생했습니다.");
        }
        else
        {
            // 응답 데이터 가져오기
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // JSON 파싱
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(responseText);

            if (!string.IsNullOrEmpty(loginResponse.token))
            {
                // 로그인 성공 시 토큰 저장
                PlayerPrefs.SetString("token", loginResponse.token);
                Debug.Log("Login successful. Token: " + loginResponse.token);

                // 로그인 성공 시 처리할 로직 호출
                LoginMgr.OnLoginSuccess(userId);
            }
            else
            {
                // 로그인 실패 처리
                Debug.Log("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");
                LoginMgr.ShowLoginWarning("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");
            }
        }
    }

    private IEnumerator LogoutCoroutine()
    {
        Debug.Log("LogoutCoroutine started"); // 디버그 로그 추가

        string token = PlayerPrefs.GetString("token");

        // POST 요청 설정
        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/logout", "POST");
        request.uploadHandler = new UploadHandlerRaw(new byte[0]); // 빈 바디로 설정
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        Debug.Log("Request sent"); // 디버그 로그 추가

        // 응답 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 응답 데이터 가져오기
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // 로그아웃 성공 시 토큰 삭제
            PlayerPrefs.DeleteKey("token");
            Debug.Log("Logged out successfully");

            // 성공 처리
            OnLogoutSuccess();
        }
    }

    private void OnLogoutSuccess()
    {
        // 로그아웃 성공 시 처리할 로직
        Debug.Log("Logout successful!");
    }

    [System.Serializable]
    public class LoginData
    {
        public string user_id;
        public string user_pwd;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string token;
        public string error;
    }
}
