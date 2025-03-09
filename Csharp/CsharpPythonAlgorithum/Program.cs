using System;
using Python.Runtime;

class Program
{
    static void Main()
    {
        var pythonPath = @"C:\Users\Park_Jun\source\repos\ConsoleApp1\packages\pythonnet.3.0.1\lib\netstandard2.0";
        // ✅ Python Home 경로 설정 (DLL 경로가 아님!)
        PythonEngine.PythonHome = pythonPath;

        // ✅ Python 환경 초기화
        PythonEngine.Initialize();

        using (Py.GIL())  // Python Global Interpreter Lock
        {
            dynamic np = Py.Import("numpy");
            dynamic kalman = Py.Import("kalman_lib");  // Python 파일 import
            dynamic results = kalman.run_kalman_filter(np.array(new double[] { 1, 2, 3, 4, 5, 6 }));

            Console.WriteLine("Filtered Values from Python:");
            foreach (var value in results)
            {
                Console.WriteLine(value);
            }
        }

        // ✅ Python 환경 종료
        PythonEngine.Shutdown();
    }
}
