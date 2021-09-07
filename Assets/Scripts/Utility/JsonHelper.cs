using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public static class JsonHelper
{
    static string dataPath = Application.dataPath + "/JsonData/";

    //Jsonとしてデータを保存。
    //同じKeyがあった場合は書き換える
    public static bool SaveData<T>(T data, string key, string name = null)
    where T:class
    {
        string json = JsonUtility.ToJson(data);
        string dirPath = dataPath + typeof(T).Name;

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        if (name == null)
        {
            name = typeof(T).Name;
        }

        string fullPath = dirPath + "/" + name;

        if (!File.Exists(fullPath))
        {
            File.Create(fullPath).Close();
        }

        UpdateData<T>(fullPath,key,key + ">" + json);

        return true;
    }



    public static T GetData<T>(string key, string name = null)
    where T : class
    {
        if (name == null)
        {
            name = typeof(T).Name;
        }

        string dirPath = dataPath + typeof(T).Name;
        string fullPath = dirPath + "/" + name;


        if (!File.Exists(fullPath))
        {
            return null;
        }

        StreamReader sr = new StreamReader(fullPath);

        return FindDataByKey<T>(sr, key);
    }


    static T FindDataByKey<T>(StreamReader sr, string key)
    where T : class
    {
        bool result = false;
        do
        {
            result = true;
            var line = sr.ReadLine();

            if (line == null)
            {
                break;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if (line[i] != key[i])
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                line = line.Replace(key + ">", "");
                sr.Close();
                return JsonUtility.FromJson<T>(line);
            }

        } while (true);

        sr.Close();
        return null;
    }
    static int FindDataIndexByKey<T>(StreamReader sr, string key)
    where T : class
    {
        int count = 0;

        bool result = false;
        do
        {
            count++;

            result = true;
            var line = sr.ReadLine();

            if (line == null)
            {
                break;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if (line[i] != key[i])
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                line = line.Replace(key + ">", "");
                sr.Close();
                return count;
            }

        } while (true);

        sr.Close();
        return -1;
    }

    //キーの場所のデータを更新。なければそのまま更新
    static bool UpdateData<T>(string path, string key, string newLine)
    where T : class
    {
        var index = 0;
        using (var sr = new StreamReader(path))
        {
            index = FindDataIndexByKey<T>(sr, key);
        }

        //見つからなかったら
        if (index == -1)
        {
            //普通にAppend
            using(var sw = new StreamWriter(path,true))
            {
                sw.WriteLine(newLine);
            }
            return false;
        }

        //全部吐き出す
        string[] oldString = File.ReadAllLines(path);

        using (var sw = new StreamWriter(path))
        {
            //指定の位置のみ更新
            for (int i = 0; i < oldString.Length; i++)
            {
                if (i == index - 1)
                {
                    sw.WriteLine(newLine);
                }
                else
                {
                    sw.WriteLine(oldString[i]);
                }
            }

        }

        return true;
    }
}

