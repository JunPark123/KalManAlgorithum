public class Program
{



    private static void Main(string[] args)
    {
        BaseClass obj = new DerivedClass();
        obj.PrintMessage(); // "DerivedClass PrintMessage"


        var D = new Derived("cONSTRUCTED IN MAIN");
        D.vFunc();
        var B = new DerivedA("cONSTRUCTED IN MAIN");
    }
}


public class BaseClass : IDisposable
{
    public virtual void Dispose()
    {
        throw new NotImplementedException();
    }

    public virtual void PrintMessage()
    {
        Console.WriteLine("BaseClass PrintMessage");
    }
}

public class DerivedClass : BaseClass
{
    public override void PrintMessage()
    {
        Console.WriteLine("DerivedClass PrintMessage");
    }
}

//정적생성자
public class MySingleton
{
    //정적멤버 초기화 구문
    private static readonly MySingleton instance = new MySingleton();
    public static MySingleton Instance { get => instance; }

    private MySingleton()
    {

    }

    //정적생성자에서 초기화하기
    private static readonly MySingleton instance2;
    static MySingleton()
    {
        instance2 = new MySingleton();
    }

    public static MySingleton Instance2 { get => instance2; }

}

public class B
{
    protected B()
    {
        //vFunc();
    }
    public virtual void vFunc()
    {
        Console.WriteLine("B입니다 vFunc()");
    }
}

public class Derived : B
{
    private readonly string msg = "Set By initializer";
    public Derived(string msg)
    {
        this.msg = msg;
        
    }

    public override void vFunc()
    {
        Console.WriteLine(msg);
    }
}

abstract class A
{
    protected A()
    {
        vFunc();
    }
    protected abstract void vFunc();
}

class DerivedA : A
{
    private readonly string msg = "Set By initializer";
    public DerivedA(string msg)
    {
        this.msg = msg;
    }

    protected override void vFunc()
    {
        Console.WriteLine(msg);
    }
}

public class Factory<T> where T:new()
{
    public T Create()
    {
        return new T();
    }
}

