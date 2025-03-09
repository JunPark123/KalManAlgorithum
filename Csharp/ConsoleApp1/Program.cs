using System;
using System.Collections.Specialized;

class MainClass
{

    public static string ArrayChallenge(string[] strArr)
    {

        //시작점 노드 고정
        string startnode = strArr[0];
        // code goes here  
        return strArr[0];

    }


    //public static int ArrayChallenge(int[] arr)
    //{
    //    int[] copyarr = new int[arr.Length];
    //    Array.Copy(arr,copyarr,arr.Length);
    //    // code goes here  
    //    int count = 0;
    //    int index = -1;
    //    int count_back = 0;

    //    for (int i = 0; i < copyarr.Length; i++)
    //    {
    //        count = 0;

    //        for (int j = i + 1; j < copyarr.Length; j++)
    //        {
    //            if (copyarr[j] == copyarr[i])
    //            {
    //                count++;
    //            }
    //        }

    //        if (count > count_back)
    //        {
    //            count_back = count;
    //            index = i;
    //        }
    //    }

    //    if (index == -1) return -1;
    //    return copyarr[index];


    //}

    static void Main()
    {
        string[] strArr = new string[] { "4", "3", "4"};
        string[] copyArr;
        int nodecnt = 1;
        int index = 1;
        string beforestr = "";
        string afterstr = "";
        

        for (int i = 1; i < strArr.Length; i = index) //여기가 시작점
        {
            
            if (nodecnt+index > strArr.Length) break;
            int count = 0;
            
            //인덱스는 1부터 시작
            nodecnt *= 2;
            copyArr = new string[nodecnt];

            for (int j = index; j < nodecnt + index; j++) //node 개수만큼 도는곳
            {
                copyArr[count] = strArr[j];
                count++;
            }

            for (int k = 0; k < count / 2; k++)
            {
                beforestr += copyArr[k];
            }

            for (int k = count - 1; k >= count / 2; k--) //4일때
            {
                afterstr += copyArr[k];
            }

            if (beforestr != afterstr)
            { 
                return; 
            }
            beforestr = "";
            afterstr = "";

            index = nodecnt + index;
        }
    }
}

