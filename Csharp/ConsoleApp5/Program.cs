using System;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
    private int count = 0;
    private int sum = 0;
    public static int solution3(int num)
    {
        if (num == 1) return 1;

        return num + solution3(num - 1);
    }
    public static int solution2(int num)
    {
        int sum = 0;
        int count = 1;
        while (true)
        {
            sum += count;
            if (count == num) break;
            count++;
        }

        return sum;
    }
    private static void Main(string[] args)
    {
        Solution(3);
        solution3(10);
        Console.WriteLine($"{solution2(10)} // {solution3(10)}");
    }

    private static void Solution(int num)
    {
        int person = 3;
        int price = 15000;

        int totalprice = 0;

        int gus10000 = 1;
        int gus5000 = 1;
        int gus1000 = 1;
        int aa = (22000 / 10000);
        //인원수 곱하기
 
        while (totalprice == price)
        {

            if (price / 10000 > 0)
            {

            }
        }


        gus10000 = price / 10000;
        price = price - 10000 * gus10000;

        if (price <= 0)
        {
            gus5000 = price / 5000; //5000원 후원한 사람 수
            price = price - 5000 * gus5000;
        }

        if (price <= 0)
        {
            gus1000 = price / 1000; //1000 후원한 사람 수
        }
    }
}