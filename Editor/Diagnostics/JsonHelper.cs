using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniTools.Build
{
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(List<T> data)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = data;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> data, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = data;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}