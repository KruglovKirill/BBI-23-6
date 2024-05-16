using ProtoBuf;

namespace ConsoleApp1.serializers;

public class MySerializerBinary<T> : MySerializer<T> where T : class
{
    public MySerializerBinary(string filename) : base(filename)
    {
    }

    public override T Read()
    {
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            // пытаемся десериализовать объект из файла, иначе отловим ошибку и вернем null
            try
            {
                return Serializer.Deserialize<T>(fs);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    public override void Write(T t)
    {
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            // сериализуем наш объект и записываем его в файл
            try
            {
                Serializer.Serialize(fs, t);
            }
            catch (Exception e) // отлавливаем исключение и выводим инфу о нем в консоль
            {
                Console.WriteLine(e);
            }
        }
    }
}