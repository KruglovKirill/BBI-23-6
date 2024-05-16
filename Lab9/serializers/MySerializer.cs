namespace ConsoleApp1.serializers;

public abstract class MySerializer<T> where T : class
{
    protected string filename;

    protected MySerializer(string filename)
    {
        this.filename = filename;
    }

    public abstract void Write(T t);

    public abstract T Read();
}