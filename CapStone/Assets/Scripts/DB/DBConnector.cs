using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using UnityEngine;

public class DBConnector : MonoBehaviour
{
    [SerializeField] private string IP = "221.166.156.80";
    [SerializeField] private int PORT = 3307;
    [SerializeField] private string ID = string.Empty;
    [SerializeField] private string PW = string.Empty;
    [SerializeField] private string DB_NAME = string.Empty;

    private static DBConnector single;
    private static MySqlConnection _connection = null;

    private static MySqlConnection connection
    {
        get
        {
            if (_connection == null)
            {
                try
                {
                    string formatSql = $"Server={single.IP}; Port={single.PORT}; Database={single.DB_NAME}; UserId={single.ID}; Password={single.PW}";
                    _connection = new MySqlConnection(formatSql);
                }
                catch (MySqlException e)
                {
                    Debug.LogError(e);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return _connection;
        }
    }

    private void Awake()
    {
        single = this;
        DontDestroyOnLoad(single);
    }

    private static bool m_OnChange(string query)
    {
        bool result = false;
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.ExecuteNonQuery();
            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        return result;
    }

    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null;
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            sd.Fill(ds, tableName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        return ds;
    }

    public static XmlNodeList Select(string tableName, string field = "*", string condition = "")
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT {field} FROM {tableName} {condition}");

        if (dataSet == null)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }

    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ('{value}')");
    }

    public static bool Insert(string tableName, string[] values)
    {
        string strValues = string.Join(", ", Array.ConvertAll(values, value => $"'{value}'"));
        return m_OnChange($"INSERT INTO {tableName} VALUES ({strValues})");
    }

    public static bool Update(string tableName, string fieldName, string value, string condition)
    {
        return m_OnChange($"UPDATE {tableName} SET {fieldName}='{value}' WHERE {condition}");
    }

    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }
}

public static class DBConnector_Expand
{
    public static T ParseXmlNode<T>(this XmlNode node, string fieldName)
    {
        if (node[fieldName] == null || string.IsNullOrEmpty(node[fieldName].InnerText))
            return default(T);

        return (T)Convert.ChangeType(node[fieldName].InnerText, typeof(T));
    }
}
