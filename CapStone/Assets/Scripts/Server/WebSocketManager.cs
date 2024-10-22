using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

public class WebSocketManager : MonoBehaviour
{
    public static WebSocketManager Instance { get; private set; } // 싱글톤 인스턴스
    
    private string apiUrl = "http://221.166.156.80:3000/api/users"; // 개발용 API URL
    private WebSocket ws;
    private string jwtToken;

    [SerializeField]
    private LoginMgr LoginMgr;

    private void Awake()
    {
        // 싱글톤 패턴 적용: 인스턴스가 존재하지 않으면 생성하고, 존재하면 파괴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 새로운 오브젝트 파괴
        }
    }

    void Start()
    {
        // 서버에 대한 WebSocket 연결 설정
        StartWebSocketConnection();
    }

    public void Login(string userId, string userPwd)
    {
        Debug.Log("Login method called");
        StartCoroutine(LoginCoroutine(userId, userPwd));
    }

    public void Logout()
    {
        Debug.Log("Logout method called");
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LoginCoroutine(string userId, string userPwd)
    {
        Debug.Log("LoginCoroutine started");

        var loginData = new LoginData { user_id = userId, user_pwd = userPwd };
        var jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Login JSON: " + jsonData);

        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Connection error: " + request.error);
            LoginMgr.ShowLoginWarning("로그인 실패: 서버 연결 오류가 발생했습니다.");
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("HTTP Error: " + request.error);
            string responseText = request.downloadHandler.text;
            Debug.Log("Error Response: " + responseText);

            if (request.responseCode == 400)
            {
                ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(responseText);
                Debug.LogError("로그인 실패: " + errorResponse.error);
                LoginMgr.ShowLoginWarning("로그인 실패: " + errorResponse.error);
            }
            else
            {
                LoginMgr.ShowLoginWarning("로그인 실패: " + request.error);
            }
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(responseText);

            if (!string.IsNullOrEmpty(loginResponse.token))
            {
                PlayerPrefs.SetString("token", loginResponse.token);
                jwtToken = loginResponse.token;
                Debug.Log("Login successful. Token: " + loginResponse.token);

                // WebSocket 연결 재시작
                StartWebSocketConnection();

                // 로그인 성공 시 처리할 로직 호출
                LoginMgr.OnLoginSuccess(userId);
            }
            else
            {
                Debug.Log("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");
                LoginMgr.ShowLoginWarning("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");
            }
        }
    }

    private IEnumerator LogoutCoroutine()
    {
        Debug.Log("LogoutCoroutine started");

        string token = PlayerPrefs.GetString("token");

        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/logout", "POST");
        request.uploadHandler = new UploadHandlerRaw(new byte[0]);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            PlayerPrefs.DeleteKey("token");
            Debug.Log("Logged out successfully");

            // WebSocket 연결 종료
            CloseWebSocketConnection();

            // 성공 처리
            OnLogoutSuccess();
        }
    }

    private void StartWebSocketConnection()
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is not set. Cannot start WebSocket connection.");
            return;
        }

        string url = "ws://221.166.156.80:3000"; // 서버 주소에 맞게 수정
        ws = new WebSocket(url);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected to server.");
            AuthenticateWithServer();
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received message from server: " + e.Data);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket connection closed.");
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket error: " + e.Message);
        };

        ws.Connect();
    }

    private void AuthenticateWithServer()
    {
        if (!string.IsNullOrEmpty(jwtToken))
        {
            var authMessage = new AuthenticateMessage
            {
                type = "authenticate",
                token = jwtToken
            };

            string jsonMessage = JsonUtility.ToJson(authMessage);
            ws.Send(jsonMessage);
            Debug.Log("Sent authentication message to server.");
        }
    }

    private void CloseWebSocketConnection()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
            Debug.Log("WebSocket connection closed.");
        }
    }

    private void OnLogoutSuccess()
    {
        Debug.Log("Logout successful!");
        // 로그아웃 후 추가 처리 로직
    }

    private void OnDestroy()
    {
        CloseWebSocketConnection();
        Debug.Log("WebSocket connection closed on destroy.");
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

    [System.Serializable]
    public class ErrorResponse
    {
        public string error;
    }

    [System.Serializable]
    public class AuthenticateMessage
    {
        public string type;
        public string token;
    }
}
