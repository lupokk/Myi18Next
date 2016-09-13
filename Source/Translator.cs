using System;
using System.Collections.Generic;
using System.IO;

namespace IToolS.Web.i18Next
{
   using Newtonsoft.Json.Linq;

   public class Translator
   {
      String _path;

      Dictionary<String, JObject> _dictionary;

      private Translator()
      {
         _dictionary = new Dictionary<string, JObject>();
      }

      public Translator(String path, String jsonFileName = "translation.json")
         : this()
      {
         _path = path;

         foreach (var folder in Directory.GetDirectories(path))
         {
            String directoryName = new DirectoryInfo(folder).Name;

            String jsonFile = Path.Combine(path, directoryName, jsonFileName);

            if (File.Exists(jsonFile))
            {
               JObject jObject = JObject.Parse(File.ReadAllText(jsonFile));

               _dictionary[directoryName] = jObject;
            }
         }
      }

      public String Translate(String culture, String text)
      {
         if (String.IsNullOrEmpty(text))
            return text;

         if (!_dictionary.ContainsKey(culture))
            return text;

         String[] fields = text.Split('.');

         JToken jToken = _dictionary[culture];

         for (int i = 0; i < fields.Length; i++)
         {
            jToken = jToken[fields[i]];

            if (jToken == null)
               return text;

            if (i == fields.Length - 1)
               return jToken.ToString();
         }

         return text;
      }
   }
}
