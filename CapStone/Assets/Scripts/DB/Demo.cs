using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Demo 클래스 정의
public class Demo : MonoBehaviour
{
    private const string DB_TABLE_USER_INFO = "user"; // 데이터베이스 테이블 이름 상수

    // Start 메서드는 처음 프레임이 업데이트되기 전에 호출됩니다.
    private void Start()
    {
        // View 메서드 정의
        void View()
        {
            // DBConnector 클래스의 Select 메서드를 사용하여 userInfo 테이블에서 데이터를 조회합니다.
            XmlNodeList userInfoList = DBConnector.Select(DB_TABLE_USER_INFO);

            // 조회된 데이터를 사용하는 방법
            foreach (XmlNode node in userInfoList)
            {
                // 각 노드에서 필드 값을 추출하여 출력합니다.
                Debug.Log(node.ParseXmlNode<int>("user_no")); // "uID" 필드의 값을 정수로 변환하여 출력
                Debug.Log(node.ParseXmlNode<string>("user_id")); // "id" 필드의 값을 문자열로 출력
            }

            Debug.Log("=================================================");
        }

        // View 메서드 호출
        View();
        /*
        // DBConnector 클래스의 Insert 메서드를 사용하여 userInfo 테이블에 데이터를 삽입합니다.
        if (DBConnector.Insert(DB_TABLE_USER_INFO, "id", "ssg88"))
        {
            Debug.Log("입력 성공");
            // 데이터를 삽입한 후 데이터를 다시 조회하여 출력합니다.
            View();
        }

        // DBConnector 클래스의 Insert 메서드를 사용하여 userInfo 테이블에 데이터를 배열 형태로 삽입합니다.
        if (DBConnector.Insert(DB_TABLE_USER_INFO, new string[] { "0", "ssg9" }))
        {
            Debug.Log("입력 성공");
            // 데이터를 삽입한 후 데이터를 다시 조회하여 출력합니다.
            View();
        }

        // DBConnector 클래스의 Update 메서드를 사용하여 userInfo 테이블의 데이터를 업데이트합니다.
        if(DBConnector.Update(DB_TABLE_USER_INFO, "id", "ssg101", "uID = 4"))
        {
            Debug.Log("변경 성공");
            // 데이터를 업데이트한 후 데이터를 다시 조회하여 출력합니다.
            View();
        }

        // DBConnector 클래스의 Delete 메서드를 사용하여 userInfo 테이블의 데이터를 제거합니다.
        if (DBConnector.Delete(DB_TABLE_USER_INFO, "id = 'ssg88'"))
        {
            Debug.Log("제거 성공");
            // 데이터를 제거한 후 데이터를 다시 조회하여 출력합니다.
            View();
        }
        */
    }
}
