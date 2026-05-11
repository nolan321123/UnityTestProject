using UnityEditor;
using UnityEngine;

public class JenkinsBuild
{
    // Android打包方法
    public static void BuildAndroid()
    {
        string[] scenes = { "Assets\FPS\Scenes\MainScene.unity" };
        string outputPath = "Builds/Android/Game.apk";
        
        BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.Android, BuildOptions.None);
        UnityEngine.Debug.Log("Android build completed: " + outputPath);
    }
    
    // Windows打包方法
    public static void BuildWindows()
    {
        string[] scenes = { "Assets\FPS\Scenes\MainScene.unity" };
        string outputPath = "Builds/Windows/Game.exe";
        
        BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        UnityEngine.Debug.Log("Windows build completed: " + outputPath);
    }
}
