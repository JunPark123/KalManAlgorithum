using System;
using Python.Runtime;

class Program
{
    static void Main()
    {
        var pythonPath = @"C:\Users\Park_Jun\AppData\Local\Programs\Python\Python313\python313.dll";
        // ✅ Python Home 경로 설정 (DLL 경로가 아님!)
        //PythonEngine.PythonHome = pythonPath;
        Runtime.PythonDLL = pythonPath;
        // ✅ Python 환경 초기화
        PythonEngine.Initialize();

        using (Py.GIL())  // Python Global Interpreter Lock
        {
            dynamic np = Py.Import("numpy");
            dynamic sys = Py.Import("sys");
            sys.path.append(@"C:\Users\Park_Jun\my_python_project");
            dynamic kalman = Py.Import("Algorithum_CalMan");

            dynamic results = kalman.run_kalman_filter(np.array(new double[] { 1, 2, 3, 4, 5, 6 }));

            Console.WriteLine("Filtered Values from Python:");
            foreach (var value in results)
            {
                Console.WriteLine(value);
            }

            Console.ReadLine();
        }

        // ✅ Python 환경 종료
        PythonEngine.Shutdown();
    }
}
