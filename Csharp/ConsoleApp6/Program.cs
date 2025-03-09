using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BMSTestSystem
{
    // 테스트 결과 열거형
    public enum TestResult
    {
        NotRun,
        Running,
        Pass,
        Fail,
        Error
    }

    // 테스트 케이스 기본 클래스 (템플릿 메소드 패턴)
    public abstract class TestCase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, object> ExpectedResults { get; set; }
        public TestResult Result { get; set; } = TestResult.NotRun;
        public string ErrorMessage { get; set; }
        public TimeSpan ExecutionTime { get; set; }

        // 템플릿 메소드 - 테스트 실행 과정의 뼈대를 정의
        public TestResult Execute()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Result = TestResult.Running;

            try
            {
                Logger.Log($"테스트 시작: {Name}");

                // 1. 테스트 초기화
                Initialize();

                // 2. 테스트 실행
                RunTest();

                // 3. 결과 검증
                ValidateResults();

                // 테스트 통과
                Result = TestResult.Pass;
                Logger.Log($"테스트 통과: {Name}");
            }
            catch (TestFailedException ex)
            {
                // 테스트 실패 (예상된 조건 불만족)
                Result = TestResult.Fail;
                ErrorMessage = ex.Message;
                Logger.Log($"테스트 실패: {Name} - {ex.Message}");
            }
            catch (Exception ex)
            {
                // 예상치 못한 오류
                Result = TestResult.Error;
                ErrorMessage = ex.Message;
                Logger.Log($"테스트 오류: {Name} - {ex.Message}");
            }
            finally
            {
                // 4. 테스트 정리
                Cleanup();

                stopwatch.Stop();
                ExecutionTime = stopwatch.Elapsed;
                Logger.Log($"테스트 완료: {Name} (소요시간: {ExecutionTime.TotalSeconds:F2}초)");
            }

            return Result;
        }

        // 하위 클래스에서 구현할 추상 메소드들
        protected abstract void Initialize();
        protected abstract void RunTest();
        protected abstract void ValidateResults();
        protected abstract void Cleanup();

        // 테스트 실패 예외
        public class TestFailedException : Exception
        {
            public TestFailedException(string message) : base(message) { }
        }
    }

    // 전압 테스트 구현 예제
    public class VoltageTest : TestCase
    {
        private double measuredVoltage;

        protected override void Initialize()
        {
            // 하드웨어 초기화 또는 테스트 환경 설정
            Logger.Log($"전압 테스트 초기화 - 파라미터: {JsonSerializer.Serialize(Parameters)}");

            // 파라미터 검증
            if (!Parameters.ContainsKey("targetVoltage") || !Parameters.ContainsKey("tolerance"))
                throw new Exception("필수 파라미터가 없습니다: targetVoltage 또는 tolerance");
        }

        protected override void RunTest()
        {
            // 실제 테스트 실행 (하드웨어 제어 및 측정)
            Logger.Log("전압 측정 중...");

            // 실제로는 하드웨어에서 전압을 측정하는 코드가 들어갈 위치
            // 여기서는 시뮬레이션을 위해 임의의 값 생성
            double targetVoltage = Convert.ToDouble(Parameters["targetVoltage"]);
            double noise = new Random().NextDouble() * 0.2 - 0.1; // -0.1 ~ 0.1 범위의 노이즈
            measuredVoltage = targetVoltage + noise;

            // 측정 지연 시뮬레이션
            Thread.Sleep(500);

            Logger.Log($"측정된 전압: {measuredVoltage:F3}V");
        }

        protected override void ValidateResults()
        {
            // 측정 결과 검증
            double targetVoltage = Convert.ToDouble(Parameters["targetVoltage"]);
            double tolerance = Convert.ToDouble(Parameters["tolerance"]);
            double minAcceptable = targetVoltage - tolerance;
            double maxAcceptable = targetVoltage + tolerance;

            Logger.Log($"허용 범위: {minAcceptable:F3}V ~ {maxAcceptable:F3}V");

            if (measuredVoltage < minAcceptable || measuredVoltage > maxAcceptable)
            {
                throw new TestFailedException(
                    $"전압이 허용 범위를 벗어났습니다. 측정값: {measuredVoltage:F3}V, " +
                    $"허용 범위: {minAcceptable:F3}V ~ {maxAcceptable:F3}V");
            }
        }

        protected override void Cleanup()
        {
            // 테스트 후 정리 작업
            Logger.Log("전압 테스트 정리 중...");
            // 실제로는 하드웨어 상태 초기화 등의 작업 수행
        }
    }

    // 전류 테스트 구현 예제
    public class CurrentTest : TestCase
    {
        private double measuredCurrent;

        protected override void Initialize()
        {
            Logger.Log($"전류 테스트 초기화 - 파라미터: {JsonSerializer.Serialize(Parameters)}");

            if (!Parameters.ContainsKey("targetCurrent") || !Parameters.ContainsKey("tolerance"))
                throw new Exception("필수 파라미터가 없습니다: targetCurrent 또는 tolerance");
        }

        protected override void RunTest()
        {
            Logger.Log("전류 측정 중...");

            double targetCurrent = Convert.ToDouble(Parameters["targetCurrent"]);
            double noise = new Random().NextDouble() * 0.1 - 0.05; // -0.05 ~ 0.05 범위의 노이즈
            measuredCurrent = targetCurrent + noise;

            Thread.Sleep(300);

            Logger.Log($"측정된 전류: {measuredCurrent:F3}A");
        }

        protected override void ValidateResults()
        {
            double targetCurrent = Convert.ToDouble(Parameters["targetCurrent"]);
            double tolerance = Convert.ToDouble(Parameters["tolerance"]);
            double minAcceptable = targetCurrent - tolerance;
            double maxAcceptable = targetCurrent + tolerance;

            Logger.Log($"허용 범위: {minAcceptable:F3}A ~ {maxAcceptable:F3}A");

            if (measuredCurrent < minAcceptable || measuredCurrent > maxAcceptable)
            {
                throw new TestFailedException(
                    $"전류가 허용 범위를 벗어났습니다. 측정값: {measuredCurrent:F3}A, " +
                    $"허용 범위: {minAcceptable:F3}A ~ {maxAcceptable:F3}A");
            }
        }

        protected override void Cleanup()
        {
            Logger.Log("전류 테스트 정리 중...");
        }
    }

    // 통신 테스트 구현 예제
    public class CommunicationTest : TestCase
    {
        private bool communicationSuccessful;

        protected override void Initialize()
        {
            Logger.Log($"통신 테스트 초기화 - 파라미터: {JsonSerializer.Serialize(Parameters)}");

            if (!Parameters.ContainsKey("protocol") || !Parameters.ContainsKey("timeout"))
                throw new Exception("필수 파라미터가 없습니다: protocol 또는 timeout");
        }

        protected override void RunTest()
        {
            Logger.Log("통신 테스트 중...");
            string protocol = Parameters["protocol"].ToString();
            int timeout = Convert.ToInt32(Parameters["timeout"]);

            // 통신 지연 시뮬레이션
            Thread.Sleep(timeout / 2);

            // 실제로는 여기서 BMS와 통신 시도
            // 90% 확률로 통신 성공 (시뮬레이션)
            communicationSuccessful = new Random().NextDouble() > 0.1;

            Logger.Log($"통신 결과: {(communicationSuccessful ? "성공" : "실패")}");
        }

        protected override void ValidateResults()
        {
            if (!communicationSuccessful)
            {
                throw new TestFailedException("BMS와의 통신에 실패했습니다.");
            }
        }

        protected override void Cleanup()
        {
            Logger.Log("통신 테스트 정리 중...");
        }
    }

    // 테스트 팩토리 (전략 패턴)
    public class TestFactory
    {
        public static TestCase CreateTest(string testType)
        {
            return testType.ToLower() switch
            {
                "voltage" => new VoltageTest(),
                "current" => new CurrentTest(),
                "communication" => new CommunicationTest(),
                _ => throw new ArgumentException($"지원하지 않는 테스트 유형: {testType}")
            };
        }
    }

    // 테스트 케이스 로더
    public class TestLoader
    {
        public static List<TestCase> LoadFromJson(string jsonFilePath)
        {
            var testCases = new List<TestCase>();

            try
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                var testCaseDefinitions = JsonSerializer.Deserialize<List<TestCaseDefinition>>(jsonContent);

                foreach (var definition in testCaseDefinitions)
                {
                    var testCase = TestFactory.CreateTest(definition.Type);
                    testCase.Id = definition.Id;
                    testCase.Name = definition.Name;
                    testCase.Description = definition.Description;
                    testCase.Parameters = definition.Parameters;
                    testCase.ExpectedResults = definition.ExpectedResults;

                    testCases.Add(testCase);
                }

                Logger.Log($"{testCases.Count}개의 테스트 케이스를 로드했습니다.");
            }
            catch (Exception ex)
            {
                Logger.Log($"테스트 케이스 로드 중 오류 발생: {ex.Message}");
                throw;
            }

            return testCases;
        }

        // JSON 파일 구조에 맞는 클래스
        private class TestCaseDefinition
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Dictionary<string, object> Parameters { get; set; }
            public Dictionary<string, object> ExpectedResults { get; set; }
        }
    }

    // 테스트 실행 엔진
    public class TestExecutionEngine
    {
        private readonly List<TestCase> testCases;
        private readonly bool stopOnFailure;
        private CancellationTokenSource cancellationSource;

        public event EventHandler<TestEventArgs> TestStarted;
        public event EventHandler<TestEventArgs> TestCompleted;
        public event EventHandler<TestSuiteEventArgs> TestSuiteCompleted;

        public TestExecutionEngine(List<TestCase> testCases, bool stopOnFailure = false)
        {
            this.testCases = testCases;
            this.stopOnFailure = stopOnFailure;
            this.cancellationSource = new CancellationTokenSource();
        }

        public async Task<TestSuiteResult> RunAsync()
        {
            var result = new TestSuiteResult
            {
                TotalTests = testCases.Count,
                StartTime = DateTime.Now
            };

            try
            {
                foreach (var testCase in testCases)
                {
                    if (cancellationSource.Token.IsCancellationRequested)
                    {
                        Logger.Log("테스트 실행이 취소되었습니다.");
                        break;
                    }

                    TestStarted?.Invoke(this, new TestEventArgs { TestCase = testCase });

                    var testResult = await Task.Run(() => testCase.Execute());

                    TestCompleted?.Invoke(this, new TestEventArgs { TestCase = testCase });

                    switch (testResult)
                    {
                        case TestResult.Pass:
                            result.PassedTests++;
                            break;
                        case TestResult.Fail:
                            result.FailedTests++;
                            if (stopOnFailure)
                            {
                                Logger.Log("실패한 테스트가 있어 테스트 실행을 중지합니다.");
                                cancellationSource.Cancel();
                            }
                            break;
                        case TestResult.Error:
                            result.ErrorTests++;
                            if (stopOnFailure)
                            {
                                Logger.Log("오류가 발생한 테스트가 있어 테스트 실행을 중지합니다.");
                                cancellationSource.Cancel();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"테스트 실행 중 예상치 못한 오류 발생: {ex.Message}");
                result.ErrorTests++;
            }
            finally
            {
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;

                TestSuiteCompleted?.Invoke(this, new TestSuiteEventArgs { Result = result });

                string summary = $"테스트 실행 결과: 총 {result.TotalTests}개 중 " +
                                $"성공 {result.PassedTests}개, " +
                                $"실패 {result.FailedTests}개, " +
                                $"오류 {result.ErrorTests}개, " +
                                $"미실행 {result.NotRunTests}개 " +
                                $"(소요시간: {result.Duration.TotalSeconds:F2}초)";
                Logger.Log(summary);
            }

            return result;
        }

        public void Cancel()
        {
            cancellationSource.Cancel();
            Logger.Log("테스트 취소 요청이 전송되었습니다.");
        }
    }

    // 테스트 이벤트 인자
    public class TestEventArgs : EventArgs
    {
        public TestCase TestCase { get; set; }
    }

    // 테스트 스위트 이벤트 인자
    public class TestSuiteEventArgs : EventArgs
    {
        public TestSuiteResult Result { get; set; }
    }

    // 테스트 스위트 결과
    public class TestSuiteResult
    {
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }
        public int FailedTests { get; set; }
        public int ErrorTests { get; set; }
        public int NotRunTests => TotalTests - (PassedTests + FailedTests + ErrorTests);
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
    }

    // 간단한 로거
    public static class Logger
    {
        public static void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
            Console.WriteLine(logEntry);

            // 실제 애플리케이션에서는 파일이나 데이터베이스에 로그 저장
        }
    }

    // 프로그램 메인 클래스
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("BMS 기능 검사 장비 소프트웨어 시작");

                // 테스트 케이스 JSON 파일 경로
                string testCasesPath = "testcases.json";

                // 테스트 케이스 로드
                var testCases = TestLoader.LoadFromJson(testCasesPath);

                // 테스트 실행 엔진 초기화
                var testEngine = new TestExecutionEngine(testCases);

                // 이벤트 핸들러 등록
                testEngine.TestStarted += (sender, e) =>
                    Console.WriteLine($"테스트 시작: {e.TestCase.Name}");

                testEngine.TestCompleted += (sender, e) =>
                    Console.WriteLine($"테스트 완료: {e.TestCase.Name} - 결과: {e.TestCase.Result}");

                testEngine.TestSuiteCompleted += (sender, e) =>
                    Console.WriteLine($"전체 테스트 완료: 성공 {e.Result.PassedTests}/{e.Result.TotalTests}");

                // 테스트 실행
                var result = await testEngine.RunAsync();

                // 결과 출력
                Console.WriteLine("\n===== 테스트 결과 요약 =====");
                Console.WriteLine($"총 테스트 수: {result.TotalTests}");
                Console.WriteLine($"통과: {result.PassedTests}");
                Console.WriteLine($"실패: {result.FailedTests}");
                Console.WriteLine($"오류: {result.ErrorTests}");
                Console.WriteLine($"미실행: {result.NotRunTests}");
                Console.WriteLine($"소요 시간: {result.Duration.TotalSeconds:F2}초");
                Console.WriteLine("==========================\n");

                // 샘플 테스트 케이스 생성 (실제로는 JSON 파일 사용)
                CreateSampleTestCasesFile(testCasesPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"프로그램 실행 중 오류 발생: {ex.Message}");
            }

            Console.WriteLine("종료하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }

        // 테스트용 샘플 JSON 파일 생성
        static void CreateSampleTestCasesFile(string filePath)
        {
            if (File.Exists(filePath))
                return;

            var testCases = new List<object>
            {
                new {
                    Id = "TC001",
                    Type = "Voltage",
                    Name = "배터리 전압 테스트",
                    Description = "배터리의 출력 전압이 정상 범위 내에 있는지 확인",
                    Parameters = new Dictionary<string, object>
                    {
                        { "targetVoltage", 3.7 },
                        { "tolerance", 0.2 }
                    },
                    ExpectedResults = new Dictionary<string, object>
                    {
                        { "withinRange", true }
                    }
                },
                new {
                    Id = "TC002",
                    Type = "Current",
                    Name = "충전 전류 테스트",
                    Description = "충전 중인 배터리의 전류가 정상 범위 내에 있는지 확인",
                    Parameters = new Dictionary<string, object>
                    {
                        { "targetCurrent", 2.0 },
                        { "tolerance", 0.3 }
                    },
                    ExpectedResults = new Dictionary<string, object>
                    {
                        { "withinRange", true }
                    }
                },
                new {
                    Id = "TC003",
                    Type = "Communication",
                    Name = "BMS 통신 테스트",
                    Description = "BMS와의 통신이 정상적으로 이루어지는지 확인",
                    Parameters = new Dictionary<string, object>
                    {
                        { "protocol", "CAN" },
                        { "timeout", 1000 }
                    },
                    ExpectedResults = new Dictionary<string, object>
                    {
                        { "connected", true }
                    }
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(testCases, options);
            File.WriteAllText(filePath, json);

            Console.WriteLine($"샘플 테스트 케이스 파일이 생성되었습니다: {filePath}");
        }
    }
}