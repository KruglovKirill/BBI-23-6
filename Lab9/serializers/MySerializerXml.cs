using System.Xml.Serialization;

namespace ConsoleApp1.serializers;

public class MySerializerXml<T> : MySerializer<T> where T : class
{
    public MySerializerXml(string filename) : base(filename)
    {
    }

    public override T Read()
    {
        // создаем объекст xmlSerializer
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            // пытаемся десериализовать объект из файла, иначе отловим ошибку и вернем null
            try
            {
                return xmlSerializer.Deserialize(fs) as T;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    public override void Write(T t)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            // сериализуем наш объект и записываем его в файл
            try
            {
                xmlSerializer.Serialize(fs, t);
            }
            catch (Exception e) // отлавливаем исключение и выводим инфу о нем в консоль
            {
                Console.WriteLine(e);
            }
        }
    }
}