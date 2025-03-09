using System;
using System.Security.Cryptography;

class MainClass
{

    public static int ArrayChallenge(int[] arr)
    {
        int maxVal = 0;
        int val = 0;

        for(int i=0; i<arr.Length; i++)
        {
            val = arr[i];
            int diffVal = 0;

            for (int j = i+1; j < arr.Length; j++)
            {
                int v = arr[j] - val;
                if (v > 0 && v > diffVal)
                {
                    diffVal = arr[j] - val;
                }
            }

            if (maxVal < diffVal)
            {
                maxVal = diffVal;
            }
        }


        // code goes here  
        return maxVal;

    }

    static void Main()
    {
        int[] arr = new int[] { 44, 30, 24, 32, 35, 30, 40, 38, 15 };
        // keep this function call here
        Console.WriteLine(ArrayChallenge(arr));

    }

}