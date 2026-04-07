using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public static class Timer
{
    private static readonly Stopwatch stopwatch = new();
    private static List<long> steps = new();

    private static readonly string filePath =
        Path.Combine(Application.persistentDataPath, "score.txt");

    public static bool IsRunning
    {
        get => stopwatch.IsRunning;
    }

    public static double ElapsedSeconds
    {
        get => stopwatch.ElapsedMilliseconds * 0.001f;
    }

    public static int StepsCount
    {
        get => steps.Count;
    }

    public static double GetStepElapsedSeconds(int index)
    {
        return steps[index] * 0.001f;
    }

    public static void Reset()
    {
        stopwatch.Reset();
        steps.Clear();
    }

    public static void Start()
    {
        stopwatch.Start();
    }

    public static void Stop()
    {
        stopwatch.Stop();
    }

    public static void Step()
    {
        steps.Add(stopwatch.ElapsedMilliseconds);
    }

    public static void Save()
    {
        if (steps.Count == 0)
            return;

        long currentFinalTime = steps[^1];

        if (File.Exists(filePath))
        {
            string encoded = File.ReadAllText(filePath);

            string decoded = Encoding.UTF8.GetString(
                System.Convert.FromBase64String(encoded)
            );

            string[] savedValues = decoded.Split(',');

            long previousFinalTime = long.Parse(savedValues[^1]);

            if (currentFinalTime >= previousFinalTime)
            {
                UnityEngine.Debug.Log("Record non battu.");
                return;
            }
        }

        string data = string.Join(",", steps);

        string encodedData = System.Convert.ToBase64String(
            Encoding.UTF8.GetBytes(data)
        );

        File.WriteAllText(filePath, encodedData);

        UnityEngine.Debug.Log("Nouveau record sauvegardé !");
    }

    public static void Load()
    {
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.Log("Aucune sauvegarde trouvée.");
            return;
        }

        string encoded = File.ReadAllText(filePath);

        string decoded = Encoding.UTF8.GetString(
            System.Convert.FromBase64String(encoded)
        );

        string[] loadedSteps = decoded.Split(',');

        steps.Clear();

        foreach (string value in loadedSteps)
        {
            if (long.TryParse(value, out long result))
            {
                steps.Add(result);
            }
        }

        UnityEngine.Debug.Log("Steps chargés : " + steps.Count);
    }
}