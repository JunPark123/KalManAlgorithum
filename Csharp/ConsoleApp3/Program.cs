using System;
using System.Linq;
using System.Reflection;

public class ParentClass
{
    public Class1 c1 { get; set; }
    public Class2 c2 { get; set; }
    public Class3 c3 { get; set; }
}

public class Class1
{
    public string Name { get; set; }
}

public class Class2
{
    public string age { get; set; }
}

public class Class3
{
    public string gender { get; set; }
}

public class PropertyFinder
{
    // 부모 클래스의 프로퍼티명에 해당하는 값을 반환하는 메서드
    public static object GetPropertyValueByName(object parentObject, string propertyName)
    {
        // 매핑 정의 (예시)
        var propertyMappings = new System.Collections.Generic.Dictionary<string, string>
        {
            { "Property1", "c1.Name" },
            { "Property2", "c2.age" },
            { "Property3", "c3.gender" }
        };

        // 매핑된 값 찾기
        if (propertyMappings.ContainsKey(propertyName))
        {
            string mappedProperty = propertyMappings[propertyName];//"Property1"

            // 부모 클래스의 타입 가져오기 
            Type parentType = parentObject.GetType();

            // 부모 객체에서 해당 프로퍼티명 (예: "c1", "c2", "c3") 찾기
            PropertyInfo propertyInfo = parentType.GetProperty(mappedProperty.Split('.')[0]);
            if (propertyInfo != null)
            {
                // 자식 클래스 객체를 가져오기
                var childObject = propertyInfo.GetValue(parentObject);

                // 자식 클래스의 속성(예: Name, age, gender)을 찾아 반환
                PropertyInfo childProperty = childObject.GetType().GetProperty(mappedProperty.Split('.')[1]);
                if (childProperty != null)
                {
                    return childProperty.GetValue(childObject);
                }
            }
        }

        return null;
    }
}

public class Program
{
    public static void Main()
    {
        // ParentClass 객체 생성
        ParentClass parent = new ParentClass
        {
            c1 = new Class1 { Name = "Class1 Value" },
            c2 = new Class2 { age = "Class2 Value" },
            c3 = new Class3 { gender = "Class3 Value" }
        };

        // 프로퍼티명에 해당하는 값 가져오기
        string propertyName = "Property1";
        var value1 = PropertyFinder.GetPropertyValueByName(parent, propertyName);
        if (value1 != null)
        {
            Console.WriteLine($"Property1 Value: {value1}"); // 출력: Class1 Value
        }

        propertyName = "Property2";
        var value2 = PropertyFinder.GetPropertyValueByName(parent, propertyName);
        if (value2 != null)
        {
            Console.WriteLine($"Property2 Value: {value2}"); // 출력: Class2 Value
        }

        propertyName = "Property3";
        var value3 = PropertyFinder.GetPropertyValueByName(parent, propertyName);
        if (value3 != null)
        {
            Console.WriteLine($"Property3 Value: {value3}"); // 출력: Class3 Value
        }

        Console.ReadLine();
    }
}