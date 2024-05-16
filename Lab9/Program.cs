using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ConsoleApp1.serializers;
using ProtoBuf;

namespace ConsoleApp1;

//1 уровень 1 задание

[Serializable]
[ProtoContract]
public class Participant
{
    [ProtoMember(1)] private string surname;
    [ProtoMember(2)] private string community;
    [ProtoMember(3)] private int result;
    [ProtoMember(4)] private bool disqualification;

    public Participant()
    {
    }

    // конструктор 
    public Participant(string surname, string community, int result1, int result2)
    {
        this.surname = surname;
        this.community = community;
        this.result = result1 + result2;
    }

    [JsonConstructor]
    public Participant(string surname, string community, int result, bool disqualification)
    {
        this.surname = surname;
        this.community = community;
        this.result = result;
        this.disqualification = disqualification;
    }

    // публичные геттеры и сеттеры (для xml, иначе не работает)
    public bool Disqualification
    {
        get => disqualification;
        set => disqualification = value;
    }

    public int Result
    {
        get => result;
        set => result = value;
    }

    public string Community
    {
        get => community;
        set => community = value;
    }

    public string Surname
    {
        get => surname;
        set => surname = value;
    }

    // статическая (потому что не привязана к конкретным объектам) фукнция сортировки массива
    // реализована сортировка пузырьком
    public static void Sort(Participant[] participants)
    {
        Participant temp;
        for (int i = 0; i < participants.Length - 1; i++)
        {
            for (int j = 0; j < participants.Length - i - 1; j++)
            {
                if (participants[j + 1].Result > participants[j].Result)
                {
                    temp = participants[j + 1];
                    participants[j + 1] = participants[j];
                    participants[j] = temp;
                }
            }
        }
    }

    // метод дисквалификации участника
    public void Disqualify()
    {
        disqualification = true;
    }

    // переопредляем метод ToString, который будет возращать строку с данными об объекте(участнике)
    public override string ToString()
    {
        return $"{surname,-15}{community,-15}{result,-15}";
    }

    // функция вывода массива участников за исключением дисквалифицированных 
    public static void PrintTable(Participant[] participants)
    {
        Console.WriteLine($"{"surname",-15}{"community",-15}{"result",-15}");
        foreach (var participant in participants)
        {
            if (!participant.Disqualification)
            {
                Console.WriteLine(participant.ToString());
            }
        }
    }
}

//2 уровень 7 задание

//базовый класс человек с полем фамилия
[ProtoContract]
[ProtoInclude(10, typeof(Sportsman1))]
[Serializable]
public class Human
{
    // протектед для доступа данного поля в классах наследниках
    [ProtoMember(1)] protected string surname;

    protected Human()
    {
    }

    // конструктор протектед, тк вызывается только в классах наслежниках
    protected Human(string surname)
    {
        this.surname = surname;
    }
}

// класс спортсмен наследуется от человека с доп полем ид
[ProtoContract]
[ProtoInclude(11, typeof(ChessPlayer))]
[Serializable]
public class Sportsman1 : Human
{
    [ProtoMember(2)]
    // протектед для доступа данного поля в классах наследниках
    protected int id;

    // статическая переменная для присваивания уникальных номеров
    private static int ID = 1;

    protected Sportsman1()
    {
    }

    // конструктор протектед, тк вызывается только в классах наслежниках
    protected Sportsman1(string surname) : base(surname)
    {
        id = ID++;
    }
}

// класс шахматист наследуется от спортсмена и имеет доп поле количество очков
[Serializable]
[ProtoContract]
public class ChessPlayer : Sportsman1
{
    [ProtoMember(3)] private double points;

    public ChessPlayer(string surname, double points) : base(surname)
    {
        this.points = points;
    }

    public ChessPlayer()
    {
    }

    // публичный геттеры и сеттеры (для работы xml)
    public string Surname
    {
        get => surname;
        set => surname = value;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public double Points
    {
        get => points;
        set => points = value;
    }

    public static void Sort(ChessPlayer[] chessPlayers)
    {
        int indx; //переменная для хранения индекса максимального элемента массива
        for (int i = 0; i < chessPlayers.Length; i++) //проходим по массиву с начала и до конца
        {
            indx = i; //считаем, что максимальный элемент имеет текущий индекс 
            for (int j = i; j < chessPlayers.Length; j++) //ищем максимальный элемент в неотсортированной части
            {
                if (chessPlayers[j].Points > chessPlayers[indx].Points)
                {
                    indx = j; //нашли в массиве число больше, чем chessPlayers[indx] - запоминаем его индекс в массиве
                }
            }

            if (chessPlayers[indx] ==
                chessPlayers[i]) //если максимальный элемент равен текущему значению - ничего не меняем
                continue;
            //меняем местами максимальный элемент и первый в неотсортированной части
            ChessPlayer temp = chessPlayers[i]; //временная переменная, чтобы не потерять значение chessPlayers[i]
            chessPlayers[i] = chessPlayers[indx];
            chessPlayers[indx] = temp;
        }
    }

    // переопредляем метод ToString, который будет возращать строку с данными об объекте(участнике)
    public override string ToString()
    {
        return $"{surname,-15}{id,-15}{points,-15}";
    }
}

//3 уровень 4 задание

// базовый класс спортсмен
[Serializable]
[ProtoContract]
[XmlInclude(typeof(SkierWoman))]
[XmlInclude(typeof(SkierMan))]
[ProtoInclude(10, typeof(SkierMan))]
[ProtoInclude(11, typeof(SkierWoman))]
public class Sportsman
{
    [XmlAttribute] [ProtoMember(1)] private string surname;
    [XmlAttribute] [ProtoMember(2)] private int result;

    // конструктор протектед, тк вызывается только в классах наслежниках
    [JsonConstructor]
    protected Sportsman(string surname, int result)
    {
        this.surname = surname;
        this.result = result;
    }

    protected Sportsman()
    {
    }

    // геттеры и сеттеры паблик для xml
    public int Result
    {
        get => result;
        set => result = value;
    }

    public string Surname
    {
        get => surname;
        set => surname = value;
    }


    // переопредляем метод ToString, который будет возращать строку с данными об объекте(спортсмене)
    public override string ToString()
    {
        return $"{surname,-15}{result,-15}";
    }

    // статическая (потому что не привязана к конкретным объектам) фукнция слияния двух массивов в один
    public static Sportsman[] Merge(Sportsman[] sportsmen1, Sportsman[] sportsmen2)
    {
        // новый массив в который попадут спортсмены из 1 и 2 массива
        Sportsman[] newSportsmen = new Sportsman[sportsmen1.Length + sportsmen2.Length];
        int i = 0;
        int j = 0;
        int k = 0;
        while (i < sportsmen1.Length || j < sportsmen2.Length)
        {
            if (i >= sportsmen1.Length || (j < sportsmen2.Length && sportsmen1[i].Result < sportsmen2[j].Result))
            {
                newSportsmen[k++] = sportsmen2[j++];
            }
            else
            {
                newSportsmen[k++] = sportsmen1[i++];
            }
        }

        return newSportsmen;
    }
}

// класс лыжник наследуемый от спортсмена
[Serializable]
[ProtoContract]
public class SkierMan : Sportsman
{
    [JsonConstructor]
    public SkierMan(string surname, int result) : base(surname, result)
    {
    }

    public SkierMan()
    {
    }

    // статическая (потому что не привязана к конкретным объектам) фукнция сортировки массива
    // реализована сортировка пузырьком
    public static void Sort(SkierMan[] skierMan)
    {
        SkierMan temp;
        for (int i = 0; i < skierMan.Length - 1; i++)
        {
            for (int j = 0; j < skierMan.Length - i - 1; j++)
            {
                if (skierMan[j + 1].Result > skierMan[j].Result)
                {
                    temp = skierMan[j + 1];
                    skierMan[j + 1] = skierMan[j];
                    skierMan[j] = temp;
                }
            }
        }
    }
}

// класс лыжница наследуемый от спортсмена
[Serializable]
[ProtoContract]
public class SkierWoman : Sportsman
{
    [JsonConstructor]
    public SkierWoman(string surname, int result) : base(surname, result)
    {
    }

    public SkierWoman()
    {
    }

    // статическая (потому что не привязана к конкретным объектам) фукнция сортировки массива
    // реализована сортировка пузырьком
    public static void Sort(SkierWoman[] skierWomen)
    {
        SkierWoman temp;
        for (int i = 0; i < skierWomen.Length - 1; i++)
        {
            for (int j = 0; j < skierWomen.Length - i - 1; j++)
            {
                if (skierWomen[j + 1].Result > skierWomen[j].Result)
                {
                    temp = skierWomen[j + 1];
                    skierWomen[j + 1] = skierWomen[j];
                    skierWomen[j] = temp;
                }
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // lvl 1
        string filenameBinary = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/data_1lvl.bin";
        string filenameJson = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/data_1lvl.json";
        string filenameXml = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/data_1lvl.xml";
        // binary
        Console.WriteLine("lvl-1");
        MySerializerBinary<Participant> mySerializerBinary = new MySerializerBinary<Participant>(filenameBinary);
        Participant participant = new Participant("sonya", "misis", 10, 11);
        mySerializerBinary.Write(participant);
        Participant participant2 = mySerializerBinary.Read();
        Console.WriteLine(participant2);
        // json
        MySerializerJson<Participant> mySerializerJson = new MySerializerJson<Participant>(filenameJson);
        mySerializerJson.Write(participant);
        Participant participant3 = mySerializerJson.Read();
        Console.WriteLine(participant3);
        // xml
        MySerializerXml<Participant> mySerializerXml = new MySerializerXml<Participant>(filenameXml);
        mySerializerXml.Write(participant);
        Participant participant4 = mySerializerXml.Read();
        Console.WriteLine(participant4);

        // lvl2 
        Console.WriteLine("lvl-2");
        string filenameBinary2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/data_2lvl.bin";
        string filenameJson2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/data_2lvl.json";
        string filenameXml2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/data_2lvl.xml";
        // binary
        MySerializerBinary<ChessPlayer> mySerializerBinary2 = new MySerializerBinary<ChessPlayer>(filenameBinary2);
        ChessPlayer chessPlayer = new ChessPlayer("Ivanov", 10);
        mySerializerBinary2.Write(chessPlayer);
        ChessPlayer chessPlayer2 = mySerializerBinary2.Read();
        Console.WriteLine(chessPlayer2);
        // json
        MySerializerJson<ChessPlayer> mySerializerJson2 = new MySerializerJson<ChessPlayer>(filenameJson2);
        mySerializerJson2.Write(chessPlayer);
        ChessPlayer chessPlayer3 = mySerializerJson2.Read();
        Console.WriteLine(chessPlayer3);
        // xml
        MySerializerXml<ChessPlayer> mySerializerXml2 = new MySerializerXml<ChessPlayer>(filenameXml2);
        mySerializerXml2.Write(chessPlayer);
        ChessPlayer chessPlayer4 = mySerializerXml2.Read();
        Console.WriteLine(chessPlayer4);


        // lvl3
        Console.WriteLine("lvl-3");
        // string filenameBinary3 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/data_3lvl.bin";
        // string filenameJson3 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/data_3lvl.json";
        // string filenameXml3 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/data_3lvl.xml";
        // // binary
        // MySerializerBinary<SkierMan> mySerializerBinary3 = new MySerializerBinary<SkierMan>(filenameBinary3);
        // SkierMan skierMan = new SkierMan("Ivanov", 200);
        // mySerializerBinary3.Write(skierMan);
        // SkierMan skierMan2 = mySerializerBinary3.Read();
        // Console.WriteLine(skierMan2);
        // // json
        // MySerializerJson<SkierMan> mySerializerJson3 = new MySerializerJson<SkierMan>(filenameJson3);
        // mySerializerJson3.Write(skierMan);
        // SkierMan skierMan3 = mySerializerJson3.Read();
        // Console.WriteLine(skierMan3);
        // // xml
        // MySerializerXml<SkierMan> mySerializerXml3 = new MySerializerXml<SkierMan>(filenameXml3);
        // mySerializerXml3.Write(skierMan);
        // SkierMan skierMan4 = mySerializerXml3.Read();
        // Console.WriteLine(skierMan4);


        string filenameBinarySkierWoman1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_woman_1.bin";
        string filenameJsonSkierWoman1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_woman_1.json";
        string filenameXmlSkierWoman1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_woman_1.xml";
        string filenameBinarySkierWoman2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_woman_2.bin";
        string filenameJsonSkierWoman2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_woman_2.json";
        string filenameXmlSkierWoman2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_woman_2.xml";
        string filenameBinarySkierMan1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_man_1.bin";
        string filenameJsonSkierMan1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_man_1.json";
        string filenameXmlSkierMan1 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_man_1.xml";
        string filenameBinarySkierMan2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_man_2.bin";
        string filenameJsonSkierMan2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_man_2.json";
        string filenameXmlSkierMan2 = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_man_2.xml";

        //  лыжницы-1
        SkierWoman[] skierWomen1 = new SkierWoman[]
        {
            new SkierWoman("Ivanova", 10),
            new SkierWoman("Petrova", 15),
            new SkierWoman("Sidorova", 13)
        };
        // лыжницы-2
        SkierWoman[] skierWomen2 = new SkierWoman[]
        {
            new SkierWoman("Ushakova", 19),
            new SkierWoman("Rubini", 9)
        };
        //лыжники-1
        SkierMan[] skierMen1 = new SkierMan[]
        {
            new SkierMan("Ivanov", 12),
            new SkierMan("Petrov", 27),
            new SkierMan("Sidorov", 17)
        };
        // лыжники-2
        SkierMan[] skierMen2 = new SkierMan[]
        {
            new SkierMan("Ushakov", 90),
            new SkierMan("Soso", 1)
        };

        // сортируем каждую группу
        SkierWoman.Sort(skierWomen1);
        SkierWoman.Sort(skierWomen2);
        SkierMan.Sort(skierMen1);
        SkierMan.Sort(skierMen2);

        MySerializerXml<SkierWoman[]> mySerializerXmlSkierWoman1 =
            new MySerializerXml<SkierWoman[]>(filenameXmlSkierWoman1);
        MySerializerXml<SkierWoman[]> mySerializerXmlSkierWoman2 =
            new MySerializerXml<SkierWoman[]>(filenameXmlSkierWoman2);
        MySerializerXml<SkierMan[]> mySerializerXmlSkierMan1 =
            new MySerializerXml<SkierMan[]>(filenameXmlSkierMan1);
        MySerializerXml<SkierMan[]> mySerializerXmlSkierMan2 =
            new MySerializerXml<SkierMan[]>(filenameXmlSkierMan2);

        MySerializerJson<SkierWoman[]> mySerializerJsonSkierWoman1 =
            new MySerializerJson<SkierWoman[]>(filenameJsonSkierWoman1);
        MySerializerJson<SkierWoman[]> mySerializerJsonSkierWoman2 =
            new MySerializerJson<SkierWoman[]>(filenameJsonSkierWoman2);
        MySerializerJson<SkierMan[]> mySerializerJsonSkierMan1 =
            new MySerializerJson<SkierMan[]>(filenameJsonSkierMan1);
        MySerializerJson<SkierMan[]> mySerializerJsonSkierMan2 =
            new MySerializerJson<SkierMan[]>(filenameJsonSkierMan2);

        MySerializerBinary<SkierWoman[]> mySerializerBinarySkierWoman1 =
            new MySerializerBinary<SkierWoman[]>(filenameBinarySkierWoman1);
        MySerializerBinary<SkierWoman[]> mySerializerBinarySkierWoman2 =
            new MySerializerBinary<SkierWoman[]>(filenameBinarySkierWoman2);
        MySerializerBinary<SkierMan[]> mySerializerBinarySkierMan1 =
            new MySerializerBinary<SkierMan[]>(filenameBinarySkierMan1);
        MySerializerBinary<SkierMan[]> mySerializerBinarySkierMan2 =
            new MySerializerBinary<SkierMan[]>(filenameBinarySkierMan2);


        mySerializerBinarySkierWoman1.Write(skierWomen1);
        mySerializerBinarySkierWoman2.Write(skierWomen2);
        mySerializerBinarySkierMan1.Write(skierMen1);
        mySerializerBinarySkierMan2.Write(skierMen2);

        mySerializerJsonSkierWoman1.Write(skierWomen1);
        mySerializerJsonSkierWoman2.Write(skierWomen2);
        mySerializerJsonSkierMan1.Write(skierMen1);
        mySerializerJsonSkierMan2.Write(skierMen2);

        mySerializerXmlSkierWoman1.Write(skierWomen1);
        mySerializerXmlSkierWoman2.Write(skierWomen2);
        mySerializerXmlSkierMan1.Write(skierMen1);
        mySerializerXmlSkierMan2.Write(skierMen2);


        skierWomen1 = mySerializerBinarySkierWoman1.Read();

        // выводим отсортированные группы в виде таблиц
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen1)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen1 = mySerializerXmlSkierWoman1.Read();

        // выводим отсортированные группы в виде таблиц
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen1)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen1 = mySerializerJsonSkierWoman1.Read();

        // выводим отсортированные группы в виде таблиц
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen1)
        {
            Console.WriteLine(skierWoman.ToString());
        }

        skierWomen2 = mySerializerBinarySkierWoman2.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen2)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen2 = mySerializerXmlSkierWoman2.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen2)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen2 = mySerializerJsonSkierWoman2.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen2)
        {
            Console.WriteLine(skierWoman.ToString());
        }

        skierMen1 = mySerializerBinarySkierMan1.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen1)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen1 = mySerializerXmlSkierMan1.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen1)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen1 = mySerializerJsonSkierMan1.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen1)
        {
            Console.WriteLine(skierMan.ToString());
        }

        skierMen2 = mySerializerBinarySkierMan2.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen2)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen2 = mySerializerXmlSkierMan2.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen2)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen2 = mySerializerJsonSkierMan2.Read();

        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen2)
        {
            Console.WriteLine(skierMan.ToString());
        }

        // объединяем все группы лыжников в 1 большую
        SkierMan[] skierMen = new SkierMan[skierMen1.Length + skierMen2.Length];

        for (int i = 0; i < skierMen.Length; i++)
        {
            if (i < skierMen1.Length)
            {
                skierMen[i] = skierMen1[i];
            }
            else
            {
                skierMen[i] = skierMen2[i - skierMen1.Length];
            }
        }

        // объединяем все группы лыжниц в 1 большую
        SkierWoman[] skierWomen = new SkierWoman[skierWomen1.Length + skierWomen2.Length];
        for (int i = 0; i < skierWomen.Length; i++)
        {
            if (i < skierWomen1.Length)
            {
                skierWomen[i] = skierWomen1[i];
            }
            else
            {
                skierWomen[i] = skierWomen2[i - skierWomen1.Length];
            }
        }

        // сортируем полученные группы
        SkierWoman.Sort(skierWomen);
        SkierMan.Sort(skierMen);

        string filenameBinarySkierWomanAll =
            "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_woman_all.bin";
        string filenameJsonSkierWomanAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_woman_all.json";
        string filenameXmlSkierWomanAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_woman_all.xml";
        string filenameBinarySkierManAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/skier_man_all.bin";
        string filenameJsonSkierManAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/skier_man_all.json";
        string filenameXmlSkierManAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/skier_man_all.xml";

        MySerializerXml<SkierWoman[]> mySerializerXmlSkierWomanAll =
            new MySerializerXml<SkierWoman[]>(filenameXmlSkierWomanAll);
        MySerializerXml<SkierMan[]> mySerializerXmlSkierManAll =
            new MySerializerXml<SkierMan[]>(filenameXmlSkierManAll);

        MySerializerJson<SkierWoman[]> mySerializerJsonSkierWomanAll =
            new MySerializerJson<SkierWoman[]>(filenameJsonSkierWomanAll);
        MySerializerJson<SkierMan[]> mySerializerJsonSkierManAll =
            new MySerializerJson<SkierMan[]>(filenameJsonSkierManAll);

        MySerializerBinary<SkierWoman[]> mySerializerBinarySkierWomanAll =
            new MySerializerBinary<SkierWoman[]>(filenameBinarySkierWomanAll);
        MySerializerBinary<SkierMan[]> mySerializerBinarySkierManAll =
            new MySerializerBinary<SkierMan[]>(filenameBinarySkierManAll);

        mySerializerBinarySkierWomanAll.Write(skierWomen);
        mySerializerBinarySkierManAll.Write(skierMen);

        mySerializerJsonSkierWomanAll.Write(skierWomen);
        mySerializerJsonSkierManAll.Write(skierMen);

        mySerializerXmlSkierWomanAll.Write(skierWomen);
        mySerializerXmlSkierManAll.Write(skierMen);

        skierMen = mySerializerBinarySkierManAll.Read();
        // выводим в виде таблиц отсортированные и объединенныне группы лыжниц и лыжников
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen = mySerializerXmlSkierManAll.Read();
        // выводим в виде таблиц отсортированные и объединенныне группы лыжниц и лыжников
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen)
        {
            Console.WriteLine(skierMan.ToString());
        }
        
        skierMen = mySerializerJsonSkierManAll.Read();
        // выводим в виде таблиц отсортированные и объединенныне группы лыжниц и лыжников
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierMan in skierMen)
        {
            Console.WriteLine(skierMan.ToString());
        }

        skierWomen = mySerializerBinarySkierWomanAll.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen = mySerializerXmlSkierWomanAll.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen)
        {
            Console.WriteLine(skierWoman.ToString());
        }
        
        skierWomen = mySerializerJsonSkierWomanAll.Read();
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var skierWoman in skierWomen)
        {
            Console.WriteLine(skierWoman.ToString());
        }

        // преобраазуем массив лыжниц в масств спортсменов
        Sportsman[] sportsmen1 = new Sportsman[skierWomen.Length];
        for (int i = 0; i < sportsmen1.Length; i++)
        {
            sportsmen1[i] = skierWomen[i];
        }

        //преобразуем масств лыжников в массив спортсменов
        Sportsman[] sportsmen2 = new Sportsman[skierMen.Length];
        for (int i = 0; i < sportsmen2.Length; i++)
        {
            sportsmen2[i] = skierMen[i];
        }

        // объединяем группы лыжников и лыжниц с помощью слияния
        Sportsman[] sportsmen = Sportsman.Merge(sportsmen1, sportsmen2);


        string filenameBinaryAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/all.bin";
        string filenameJsonAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/all.json";
        string filenameXmlAll = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/all.xml";

        MySerializerXml<Sportsman[]> mySerializerXmlAll =
            new MySerializerXml<Sportsman[]>(filenameXmlAll);

        MySerializerJson<Sportsman[]> mySerializerJsonAll =
            new MySerializerJson<Sportsman[]>(filenameJsonAll);

        MySerializerBinary<Sportsman[]> mySerializerBinaryAll =
            new MySerializerBinary<Sportsman[]>(filenameBinaryAll);

        mySerializerBinaryAll.Write(sportsmen);
        mySerializerJsonAll.Write(sportsmen);
        mySerializerXmlAll.Write(sportsmen);


        sportsmen = mySerializerBinaryAll.Read();
        // выовдим итоговую таблицу с лыжниками и лыжницами
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var sportsman in sportsmen)
        {
            Console.WriteLine(sportsman.ToString());
        }
        
        sportsmen = mySerializerXmlAll.Read();
        // выовдим итоговую таблицу с лыжниками и лыжницами
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var sportsman in sportsmen)
        {
            Console.WriteLine(sportsman.ToString());
        }
        
        sportsmen = mySerializerJsonAll.Read();
        // выовдим итоговую таблицу с лыжниками и лыжницами
        Console.WriteLine();
        Console.WriteLine($"{"surname",-15}{"result",-15}");
        foreach (var sportsman in sportsmen)
        {
            Console.WriteLine(sportsman.ToString());
        }

        
        // доработка
        

        Participant[] participants = new[]
        {
            new Participant("Ivanov", "MISIS1", 100, 200),
            new Participant("Petrov", "MISIS2", 1, 2000),
            new Participant("Sidorov", "MISIS3", 10, 200),
            new Participant("Pupkin", "MISIS4", 52, 67),
            new Participant("Sorokin", "MISIS5", 10099, 20),
        };
        Random random = new Random();
        participants[random.Next() % 5].Disqualify();
        participants[random.Next() % 5].Disqualify();
        Participant.Sort(participants);
        Participant.PrintTable(participants);
        
        
        string filenameBinaryParticipant = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/participants.bin";
        string filenameJsonParticipant = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/participants.json";
        string filenameXmlParticipant = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/participants.xml";

        MySerializerXml<Participant[]> mySerializerXmlParticipants =
            new MySerializerXml<Participant[]>(filenameXmlParticipant);
        MySerializerJson<Participant[]> mySerializerJsonParticipants =
            new MySerializerJson<Participant[]>(filenameJsonParticipant);
        MySerializerBinary<Participant[]> mySerializerBinaryParticipant =
            new MySerializerBinary<Participant[]>(filenameBinaryParticipant);
        
        mySerializerBinaryParticipant.Write(participants);
        mySerializerJsonParticipants.Write(participants);
        mySerializerXmlParticipants.Write(participants);

        participants = mySerializerBinaryParticipant.Read();
        Console.WriteLine("Participants:");
        foreach (var part in participants)
        {
            Console.WriteLine(participant);
        }
        
        participants = mySerializerXmlParticipants.Read();
        Console.WriteLine("Participants:");
        foreach (var part in participants)
        {
            Console.WriteLine(participant);
        }
        
        participants = mySerializerJsonParticipants.Read();
        Console.WriteLine("Participants:");
        foreach (var part in participants)
        {
            Console.WriteLine(participant);
        }
        

        ChessPlayer[] chessPlayers = new[]
        {
            new ChessPlayer("Ivanov", 15.5),
            new ChessPlayer("Petrov", 15),
            new ChessPlayer("Sidorov", 16.5),
            new ChessPlayer("Soso", 10),
            new ChessPlayer("Ronaldo", 17.5)
        };
        ChessPlayer.Sort(chessPlayers);
        
        
        string filenameBinaryChessPlayers = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/binary/ChessPlayers.bin";
        string filenameJsonChessPlayers = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/JSON/ChessPlayers.json";
        string filenameXmlChessPlayers = "/Users/excellent/Projects/Lab9/ConsoleApp1/files/XML/ChessPlayers.xml";

        MySerializerXml<ChessPlayer[]> mySerializerXmlChessPlayers =
            new MySerializerXml<ChessPlayer[]>(filenameXmlChessPlayers);
        MySerializerJson<ChessPlayer[]> mySerializerJsonChessPlayers =
            new MySerializerJson<ChessPlayer[]>(filenameJsonChessPlayers);
        MySerializerBinary<ChessPlayer[]> mySerializerBinaryChessPlayers =
            new MySerializerBinary<ChessPlayer[]>(filenameBinaryChessPlayers);
        
        mySerializerBinaryChessPlayers.Write(chessPlayers);
        mySerializerJsonChessPlayers.Write(chessPlayers);
        mySerializerXmlChessPlayers.Write(chessPlayers);

        chessPlayers = mySerializerBinaryChessPlayers.Read();
        Console.WriteLine("ChessPlayers: ");
        foreach (var player in chessPlayers)
        {
            Console.WriteLine(player);
        }
        
        chessPlayers = mySerializerXmlChessPlayers.Read();
        Console.WriteLine("ChessPlayers: ");
        foreach (var player in chessPlayers)
        {
            Console.WriteLine(player);
        }
        
        chessPlayers = mySerializerJsonChessPlayers.Read();
        Console.WriteLine("ChessPlayers: ");
        foreach (var player in chessPlayers)
        {
            Console.WriteLine(player);
        }
    }
}