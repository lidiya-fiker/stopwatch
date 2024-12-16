using System;
using System.Threading;

public delegate void StopwatchEventHandler(string message);

public class Stopwatch
{
    private TimeSpan _timeElapsed;
    private bool _isRunning;
    private DateTime _startTime;

   
    public event StopwatchEventHandler? OnStarted;
    public event StopwatchEventHandler? OnStopped;
    public event StopwatchEventHandler? OnReset;

  
    public TimeSpan TimeElapsed => _timeElapsed;
    public bool IsRunning => _isRunning;

    public void Start()
    {
        if (_isRunning)
        {
            OnStarted?.Invoke("Stopwatch is already running.");
            return;
        }

        _isRunning = true;
        _startTime = DateTime.Now - _timeElapsed;  
        OnStarted?.Invoke("Stopwatch Started!");
    }

    
    public void Stop()
    {
        if (!_isRunning)
        {
            OnStopped?.Invoke("Stopwatch is not running.");
            return;
        }

        _isRunning = false;
        _timeElapsed = DateTime.Now - _startTime;  
        OnStopped?.Invoke("Stopwatch Stopped!");
    }

  
    public void Reset()
    {
        _timeElapsed = TimeSpan.Zero;
        _isRunning = false;
        OnReset?.Invoke("Stopwatch Reset!");
    }

 
    public void Tick()
    {
        if (_isRunning)
        {
            _timeElapsed = DateTime.Now - _startTime;
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

       
        stopwatch.OnStarted += message => PrintMessage(message, ConsoleColor.Green);
        stopwatch.OnStopped += message => PrintMessage(message, ConsoleColor.Yellow);
        stopwatch.OnReset += message => PrintMessage(message, ConsoleColor.Cyan);

        Console.Clear();
        DisplayHeader();

        bool quit = false;
        while (!quit)
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Tick();
            }

            PrintElapsedTime(stopwatch.TimeElapsed); 
            Thread.Sleep(100);  

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.S:
                        stopwatch.Start();
                        break;
                    case ConsoleKey.T:
                        stopwatch.Stop();
                        break;
                    case ConsoleKey.R:
                        stopwatch.Reset();
                        break;
                    case ConsoleKey.Q:
                        quit = true;
                        break;
                    default:
                        PrintMessage("Invalid Command! Please use S, T, R, or Q.", ConsoleColor.Red);
                        break;
                }
            }
        }

        Console.Clear();
        PrintMessage("Thank you for using the Stopwatch Application. Goodbye!", ConsoleColor.Magenta);
    }

    static void DisplayHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================");
        Console.WriteLine("         Welcome to the Console Stopwatch       ");
        Console.WriteLine("===============================================");
        Console.ResetColor();

        Console.WriteLine("\nAvailable Commands: ");
        Console.WriteLine("  [S] Start the Stopwatch");
        Console.WriteLine("  [T] Stop the Stopwatch");
        Console.WriteLine("  [R] Reset the Stopwatch");
        Console.WriteLine("  [Q] Quit the Application"); 
        Console.WriteLine("\nPress a key to begin...\n");
    }

    static void PrintElapsedTime(TimeSpan elapsed)
    {
        Console.SetCursorPosition(0, 10);
        Console.Write(new string(' ', Console.WindowWidth));  
        Console.SetCursorPosition(0, 10);  

        
        string formattedTime = elapsed.TotalHours >= 1
            ? $"{elapsed:hh\\:mm\\:ss}"  
            : $"{elapsed:mm\\:ss}";       

 
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Elapsed Time: {formattedTime}");
        Console.ResetColor();
    }

    static void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}
