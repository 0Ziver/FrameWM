using Newtonsoft.Json;

namespace Frame.helpers
{
    public static class JsonUtil
    {
        public static T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            string  jsonContent = File.ReadAllText(filePath);
            T? data = JsonConvert.DeserializeObject<T>(jsonContent);
            return data;
        }

        public static void WriteToFile<T>(string filePath, T data)
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}