using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


namespace EuronewsSub.Utils
{
    public class FileReader
    {
        public static JObject ReadFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return (JObject)JToken.ReadFrom(reader);
            }
        }
    }
}
