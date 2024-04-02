using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

// DBConnector 클래스 정의
public class DBConnector : MonoBehaviour
{
    // 데이터베이스 연결을 위한 변수들
    [SerializeField] private string IP = "221.166.156.80";
    [SerializeField] private int PORT = 3306;
    [SerializeField] private string ID = string.Empty;
    [SerializeField] private string PW = string.Empty;
    [SerializeField] private string DB_NAME = string.Empty;

    private static DBConnector single; // DBConnector 클래스의 단일 인스턴스를 가리키는 변수

    private static MySqlConnection _connection = null; // MySQL 데이터베이스 연결을 위한 MySqlConnection 객체
    private static MySqlConnection connection // MySqlConnection 객체에 대한 getter
    {
        get
        {
            // 연결이 없을 경우에만 연결을 시도하고 MySqlConnection 객체를 생성
            if(_connection == null)
            {
                try
                {
                    // 데이터베이스 연결 문자열 생성
                    string formatSql = $"Server={single.IP}; Port={single.PORT}; Database={single.DB_NAME}; UserId={single.ID}; Password={single.PW}";
                    // MySqlConnection 객체 생성
                    _connection = new MySqlConnection(formatSql);
                }
                catch(MySqlException e)
                {
                    Debug.LogError(e); // MySQL 예외 처리
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex); // 일반 예외 처리
                }
            }

            return _connection; // MySqlConnection 객체 반환
        }
    }

    // MonoBehaviour의 Awake() 메서드 오버라이드
    private void Awake()
    {
        single = this; // 인스턴스를 설정
        DontDestroyOnLoad(single); // 씬 전환 시에도 유지되도록 설정
    }

    // 데이터 변경 메서드
    private static bool m_OnChange(string query)
    {
        bool result = false; // 변경 결과를 저장할 변수 초기화
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand(); // MySqlCommand 객체 생성
            sqlCommand.Connection = connection; // MySqlConnection 객체 할당
            sqlCommand.CommandText = query; // 쿼리 문자열 할당

            connection.Open(); // 데이터베이스 연결 오픈

            sqlCommand.ExecuteNonQuery(); // 쿼리 실행

            connection.Close(); // 데이터베이스 연결 닫기

            result = true; // 결과를 성공으로 설정
        }
        catch (Exception e)
        {   
            Debug.LogError(e.ToString()); // 예외 처리
        }

        connection.Close(); // 데이터베이스 연결 닫기
        return result; // 결과 반환
    }

    // 데이터 로드 메서드
    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; // DataSet 객체 초기화
        try
        {
            connection.Open();   // 데이터베이스 연결 오픈

            MySqlCommand cmd = new MySqlCommand(); // MySqlCommand 객체 생성
            cmd.Connection = connection; // MySqlConnection 객체 할당
            cmd.CommandText = query; // 쿼리 문자열 할당

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd); // MySqlDataAdapter 객체 생성
            ds = new DataSet(); // DataSet 객체 생성
            sd.Fill(ds, tableName); // 데이터 채우기
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString()); // 예외 처리
        }

        connection.Close();  // 데이터베이스 연결 닫기
        return ds; // DataSet 반환
    }

    // 데이터 조회 메서드
    public static XmlNodeList Select(string tableName, string field = "*", string condition = "")
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT {field} FROM {tableName} {condition}"); // 데이터 로드

        if (dataSet == null)
            return null;

        XmlDocument xmlDocument = new XmlDocument(); // XmlDocument 객체 생성
        xmlDocument.LoadXml(dataSet.GetXml()); // XML로드

        return xmlDocument.GetElementsByTagName(tableName); // XML 요소 반환
    }

    // 데이터 삽입 메서드
    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ('{value}')"); // 데이터 삽입
    }

    // 데이터 삽입 메서드 (배열 형태로)
    public static bool Insert(string tableName, string[] values)
    {
        string strValues = string.Empty; // 값을 저장할 변수 초기화

        foreach (string value in values)
        {
            if (strValues.Length > 0)
                strValues += ", ";
            strValues += $"'{value}'"; // 값 추가
        }

        return m_OnChange($"INSERT INTO {tableName} VALUES ({strValues})"); // 데이터 삽입
    }

    // 데이터 업데이트 메서드
    public static bool Update(string tableName, string fieldName, string value, string condition)
    {
        return m_OnChange($"UPDATE {tableName} SET {fieldName}='{value}' WHERE {condition}"); // 데이터 업데이트
    }

    // 데이터 삭제 메서드
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}"); // 데이터 삭제
    }
}

// DBConnecter_Expand 클래스 정의
public static class DBConnecter_Expand
{
    // XmlNode로부터 필드 값을 파싱하는 확장 메서드
    public static T ParseXmlNode<T>(this XmlNode node, string fieldName)
    {
        return (T)System.Convert.ChangeType(node[fieldName].InnerText, typeof(T)); // XmlNode에서 필드 값을 추출하고 해당 타입으로 변환하여 반환
    }
}
