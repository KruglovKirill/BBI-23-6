namespace ConsoleApp1;
 
// base class
public abstract class Task
{
    protected string text;
    public abstract override string ToString();
 
    protected Task(string text)
    {
        this.text = text;
    }
}
 
// 1 task
public class Task1 : Task
{
    // 0 - а, 1 - б, 2 - в ...
    // буквы ё - нет почему-то :(
    private int[] _cnt = new int[32];
    private int _allCnt;
 
    public Task1(string text) : base(text)
    {
        FillCnt();
    }
 
    private void FillCnt()
    {
        for (var i = 0; i < text.Length; i++)
        {
            // используем статические методы из класса Char для работы с символами(char)
            // в условие (специально делаем все буквы маленькими, чтобы не париться с регистрами)
            // проверяем что наш символ лежит лежит в диапазоне строчных русских букв
            // символы (char) можно сравнивать, тк берутся значения из таблицы ASCII
            if (Char.ToLower(text[i]) >= 'а' && Char.ToLower(text[i]) <= 'я')
            {
                _cnt[Char.ToLower(text[i]) - 'а']++;
                _allCnt++;
            }
        }
    }
 
    public override string ToString()
    {
        String ans = "";
        for (int i = 0; i < _cnt.Length; i++)
        {
            // используем приведение к типу char и к типу дабл(чтобы деление было нецелочисленное)
            // также используем символ перевода строки - \n
            ans += Char.ToString((char)(i + 'а')) + ":" + (double)_cnt[i] / _allCnt + "\n";
        }
 
        return ans;
    }
}
 
 
//Task3
public class Task3 : Task
{
    // будем закидывать новую строку длины не более 50
    private List<string> _list = new List<string>();
 
    public Task3(string text) : base(text)
    {
        SplitText();
    }
 
    private void SplitText()
    {
        // разбили текст по пробелам
        // собираем строки из слов и пробелов между ними длинны не более 50
        // когда собрали, добавляем в наш лист
        // в конце нужно добавить собирающую строку,
        // тк в любом случае в ней останется остаток от собирания строки из слов
        string[] splitText = text.Split(" ");
        string nowString = splitText[0];
        int cnt = splitText.Length;
        for (int i = 1; i < splitText.Length; i++)
        {
            if (cnt + splitText[i].Length + 1 > 50)
            {
                _list.Add(nowString);
                nowString = splitText[i];
                cnt = splitText[i].Length;
            }
            else
            {
                cnt += splitText[i].Length + 1;
                nowString += " " + splitText[i];
            }
        }
 
        _list.Add(nowString);
    }
 
    public override string ToString()
    {
        string ans = "";
        for (var i = 0; i < _list.Count; i++)
        {
            ans += _list[i] + " - Длинна строки: " + _list[i].Length + "\n";
        }
 
        return ans;
    }
}
 
//Task6
public class Task6 : Task
{
    // работаем только с русскими словами
    private string _sym = "ауоиэыяюе";
    // считаем, что больше 20 слогов быть не может)
    private int[] _cnt = new int[20];
    
    public Task6(string text) : base(text)
    {
        FillCnt();
    }
 
    // заполняем массив, в котором индекс соответствует кол-ву слогов,
    // а значение кол-во слов с таким количеством слогов
    private void FillCnt()
    {
        string[] words = text.Split(" ");
        foreach (var word in words)
        {
            if (!Char.IsDigit(word[0]))
            {
                _cnt[CntSym(word.ToLower())]++;
            }
        }
    }
 
    // находим количество слогов в слове подсчетом количества гласных, ведь кол-во гласных = кол-во слогов
    private int CntSym(string word)
    {
        int cnt = 0;
        for (int i = 0; i < word.Length; i++)
        {
            if (InSym(word[i]))
            {
                cnt++;
            }
        }
 
        return cnt;
    }
 
    // проверяем что данная буква является гласной
    private bool InSym(char c)
    {
        for (int i = 0; i < _sym.Length; i++)
        {
            if (_sym[i] == c)
            {
                return true;
            }
        }
 
        return false;
    }
    
    public override string ToString()
    {
        string ans = "";
        for (int i = 0; i < _cnt.Length; i++)
        {
            if (_cnt[i] > 0)
            {
                ans += i + " : " + _cnt[i] + "\n";
            }
        }
        return ans;
    }
}
 
 
//Task 12
public class Task12 : Task
{
    // условный словарь с кодами, слов
    private Dictionary<String, String> _encoding = new Dictionary<string, string>();
    // и наооборот
    private Dictionary<String, String> _decoding = new Dictionary<string, string>();
    private string[] _words;
    
    public Task12(string text) : base(text)
    {
        FillDictionaries();
        _words = text.Split(" ");
        ToEncode();
    }
 
    public string GetEncodeText()
    {
        string ans = _words[0];
        for (int i = 1; i < _words.Length; i++)
        {
            ans += " " + _words[i];
        }
 
        return ans;
    }
 
    private void FillDictionaries()
    {
        // какое-то условное заполнение :)
        _encoding.Add("яблоко", "*");
        _encoding.Add("мусор", "!");
        _decoding.Add("*", "яблоко");
        _decoding.Add("!", "мусор");
    }
 
    // кодируем массив
    private void ToEncode()
    {
        for (int i = 0; i < _words.Length; i++)
        {
            // проверяем, что такой ключ есть в словаре (то есть у такого слова есть код)
            // тогда кодируем
            if (_encoding.ContainsKey(_words[i].ToLower()))
            {
                _words[i] = _encoding[_words[i].ToLower()];
            }
        }
    }
 
    // декодируем и возвращаем новый
    private string[] ToDecode()
    {
        string[] wordsDecode = new string[_words.Length];
        for (int i = 0; i < wordsDecode.Length; i++)
        {
            if (_decoding.ContainsKey(_words[i].ToLower()))
            {
                wordsDecode[i] = _decoding[_words[i]];
            }
            else
            {
                wordsDecode[i] = _words[i];
            }
        }
 
        return wordsDecode;
    }
    
    
    
    public override string ToString()
    {
        string[] wordsDecode = ToDecode();
        string ans = wordsDecode[0];
        for (int i = 1; i < wordsDecode.Length; i++)
        {
            ans += " " + wordsDecode[i];
        }
 
        return ans;
    }
}
 
 
public class Task13 : Task
{
    private int[] _cntRu = new int[32];
    private int[] _cntEn = new int[26];
    private int _numberWords;
 
    public Task13(string text) : base(text)
    {
        CountLettersStartWords();
    }
 
    // подсчитываем количество слов, начинающихся на разные буквы
    private void CountLettersStartWords()
    {
        string[] words = text.Split(" ");
        for (int i = 0; i < words.Length; i++)
        {
            // проверяем что слово не пустое и начинается на русскую букву
            if (words[i] != "" && words[i].ToLower()[0] >= 'а' && words[i].ToLower()[0] <= 'я')
            {
                // добавляем в наш массив подсчета
                _cntRu[words[i].ToLower()[0] - 'а']++;
                // считаем общее кол-во слов
                _numberWords++;
            }
            else if (words[i].ToLower()[0] >= 'a' && words[i].ToLower()[0] <= 'z')
            {
                _cntEn[words[i].ToLower()[0] - 'a']++;
                _numberWords++;
            }
        }
    }
 
    public override string ToString()
    {
        string ans = "";
        for (int i = 0; i < _cntEn.Length; i++)
        {
            if (_cntEn[i] > 0)
            {
                ans += Char.ToString((char)(i + 'a')) + ": " + (double)_cntEn[i] / _numberWords * 100 + "%\n";

            }
        }

        for (int i = 0; i < _cntRu.Length; i++)
        {
            if (_cntRu[i] > 0)
            {
                ans += Char.ToString((char)(i + 'а')) + ": " + (double)_cntRu[i] / _numberWords * 100 + "%\n";
            }
        }

        return ans;
    }
}
 
// 15 task
public class Task15 : Task
{
    private long _sumNumbers;
 
    public Task15(string text) : base(text)
    {
        SumNumbersInText();
    }
 
    // функция нахождения чисел в строке и суммирования их
    private void SumNumbersInText()
    {
        for (int i = 0; i < text.Length; i++)
        {
            string stringNumber = "";
            // формируем число-строку пока мы встречаем символ-цифру или пока не дошли до конца 
            while (i < text.Length && Char.IsDigit(text[i]))
            {
                stringNumber += text[i++];
            }
            // если получилось собрать число, преобразуем его в число (long - число до 4 * 10**18)
            // и добавляет его в сумму
            if (!stringNumber.Equals(""))
            {
                _sumNumbers += long.Parse(stringNumber);
            }
        }
    }
 
    // getter for sumNumber
    public long GetSumNumbers()
    {
        return _sumNumbers;
    }
 
    public override string ToString()
    {
        return $"Сумма чисел в тексте: {_sumNumbers}";
    }
}
 
public class Program
{
    public static void Main(string[] args)
    {
        // Test for task 12
        Task12 task12 = new Task12(
            "Яблоко мусор да");
        Console.WriteLine(task12.GetEncodeText());
        Console.WriteLine(task12.ToString());
        
        // test for task 13
        Task task13 = new Task13("яблоко мусор капец я в рот таскал");
        Console.WriteLine(task13.ToString());
    }
}
