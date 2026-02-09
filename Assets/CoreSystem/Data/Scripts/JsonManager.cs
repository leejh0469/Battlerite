using System.IO;
using System.Text;
using UnityEngine;


namespace CoreSystem.Data
{
    public static class JsonManager
    {
        private static string SavePath => Application.persistentDataPath;

        public static void Save<T>(T data, string fileName)
        {
            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string fullPath = Path.Combine(SavePath, fileName);

            try
            {
                string jsonString = JsonUtility.ToJson(data, true);

                File.WriteAllText(fullPath, jsonString, Encoding.UTF8);

                Debug.Log($"[JsonManager] 저장 성공! 위치: {fullPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] 저장 실패 ({fileName}): {e.Message}");
            }
        }

        public static T Load<T>(string fileName)
        {
            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string fullPath = Path.Combine(SavePath, fileName);

            if (!File.Exists(fullPath))
            {
                return default(T);
            }

            try
            {
                string jsonString = File.ReadAllText(fullPath, Encoding.UTF8);
                return JsonUtility.FromJson<T>(jsonString);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] 로드 실패 ({fileName}): {e.Message}");
                return default(T);
            }
        }

        public static T LoadFromResources<T>(string path)
        {
            path = path.Replace(".json", "");

            TextAsset textAsset = Resources.Load<TextAsset>(path);

            if( textAsset == null )
            {
                Debug.LogError($"[JsonManager] Resources 폴더에서 파일을 찾을 수 없음: {path}");
                return default(T);
            }

            return JsonUtility.FromJson<T>(textAsset.text);
        }
    }
}