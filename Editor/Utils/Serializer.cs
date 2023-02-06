using Editor.Exceptions;
using System.IO;
using System.Runtime.Serialization;

namespace Editor.Utils
{
    public static class Serializer
    {
        public static T FromFile<T>(string path)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Open);
                var serializer = new DataContractSerializer(typeof(T));
                T instance = (T)serializer.ReadObject(fileStream);

                return instance;
            }
            catch (WriteToFileException ex)
            {
                throw ex;
            }
        }

        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));

                serializer.WriteObject(fileStream, instance);
            }
            catch (WriteToFileException ex)
            {
                throw ex;
            }
        }
    }
}
