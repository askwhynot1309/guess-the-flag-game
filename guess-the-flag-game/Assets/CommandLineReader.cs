using System;
using UnityEngine;

public class CommandLineReader : MonoBehaviour
{
    public static string UserId { get; private set; }
    public static string FloorId { get; private set; }
    public static string GameId { get; private set; }
    public static string AccessToken { get; private set; }

    void Awake()
    {
        string[] args = Environment.GetCommandLineArgs();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--userId="))
                UserId = arg.Substring("--userId=".Length);

            if (arg.StartsWith("--floorId="))
                FloorId = arg.Substring("--floorId=".Length);

            if (arg.StartsWith("--gameId="))
                GameId = arg.Substring("--gameId=".Length);

            if (arg.StartsWith("--accessToken="))
                AccessToken = arg.Substring("--accessToken=".Length);
        }
        #if UNITY_EDITOR
                if (string.IsNullOrEmpty(UserId)) UserId = "3cd94c6a-7bbe-4b08-bd4e-961ec9e41930";
                if (string.IsNullOrEmpty(FloorId)) FloorId = "1";
                if (string.IsNullOrEmpty(GameId)) GameId = "b834ad5204834367a06be32cb4baf818    ";
                if (string.IsNullOrEmpty(AccessToken)) AccessToken = "abc";
        #endif

        Debug.Log($"[Startup Args] userId: {UserId}, floorId: {FloorId}, gameId: {GameId}, accessToken: {AccessToken}");
    }
}
