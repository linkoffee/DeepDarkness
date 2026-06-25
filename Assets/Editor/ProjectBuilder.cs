using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Reporting;

public class ProjectBuilder
{
    public enum CompressionOption
    {
        None,   // without compression (quick building, large build size)
        LZ4,    // quick compression (debug, testing only)
        LZ4HC   // max compression (slow building, small build size) - for release only
    }

    private static CompressionOption defaultCompression = CompressionOption.LZ4HC;

    private const string TimestampFormat = "dd-MM-yyyyTHH-mm-ss";
    private const string BuildFolderName = "__Builds";

    [MenuItem("Build/Windows 64-bit")]
    public static void BuildWin64()
    {
        BuildForPlatform(
            platform: "win-64",
            buildTarget: BuildTarget.StandaloneWindows64,
            extension: ".exe"
        );
    }

    [MenuItem("Build/Windows 32-bit")]
    public static void BuildWin32()
    {
        BuildForPlatform(
            platform: "win-32",
            buildTarget: BuildTarget.StandaloneWindows,
            extension: ".exe"
        );
    }

    [MenuItem("Build/Linux 64-bit")]
    public static void BuildLinux()
    {
        BuildForPlatform(
            platform: "linux-64",
            buildTarget: BuildTarget.StandaloneLinux64,
            extension: ""
        );
    }

    [MenuItem("Build/MacOS")]
    public static void BuildMacOS()
    {
        BuildForPlatform(
            platform: "macOS",
            buildTarget: BuildTarget.StandaloneOSX,
            extension: ".app"
        );
    }

    private static void BuildForPlatform(string platform, BuildTarget buildTarget, string extension)
    {
        try
        {
            string timestamp = System.DateTime.Now.ToString(TimestampFormat);
            string buildName = $"DeepDarkness-{platform}-{timestamp}";
            string buildPath = Path.Combine(BuildFolderName, buildName);

            if (!Directory.Exists(buildPath))
                Directory.CreateDirectory(buildPath);

            string executableName = $"DeepDarkness{extension}";

            string fullPath = Path.Combine(buildPath, executableName);

            if (buildTarget == BuildTarget.StandaloneOSX)
                fullPath = Path.Combine(buildPath, "DeepDarkness.app");

            string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (scenes.Length == 0)
            {
                string errorMsg = "There are no active scenes to build! Add scenes in Build Settings.";

                Debug.LogError(errorMsg);
                EditorUtility.DisplayDialog("Build error", errorMsg, "OK");
                return;
            }

            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = fullPath,
                target = buildTarget,
                targetGroup = BuildTargetGroup.Standalone,
                options = GetCompressionOptions()
            };
            
            ConfigureCompression(buildTarget);

            Debug.Log($"Build the project: {buildName} | compression mode: {defaultCompression}");
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildOptions);

            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                string buildDirectory = Path.GetDirectoryName(fullPath);
                string buildSize = GetBuildSize(buildDirectory);

                Debug.Log("The project build has been completed successfully.");
                Debug.Log($"Build size: {buildSize}");
                EditorUtility.DisplayDialog("Build Completed", $"The project build has been completed successfully\n\nBuild size: {buildSize}", "OK");
            }
            else
            {
                string errorMsg = $"Build process error: {buildReport.summary.result}";
                Debug.LogError(errorMsg);

                foreach (var step in buildReport.steps)
                {
                    bool hasError = false;
                    foreach (var message in step.messages)
                    {
                        if (message.type == LogType.Error || message.type == LogType.Exception)
                        {
                            if (!hasError)
                            {
                                Debug.LogError($"  → Error on step: {step.name}");
                                hasError = true;
                            }
                            Debug.LogError($"    {message.content}");
                        }
                    }
                }

                EditorUtility.DisplayDialog("Build Error", $"The build failed with an error: {buildReport.summary.result}\n\nCheck console log for details...", "OK");
            }
        }
        catch (System.Exception exc)
        {
            string errorMsg = $"Critical error: {exc.Message}";
            Debug.LogError(errorMsg);
            Debug.LogError(exc.StackTrace);

            EditorUtility.DisplayDialog("Critical Error", $"The build failed with an error:\n\n{exc.Message}\n\nCheck console log for details...", "OK");
        }
    }

    private static BuildOptions GetCompressionOptions()
    {
        switch (defaultCompression)
        {
            case CompressionOption.LZ4:
                return BuildOptions.CompressWithLz4;
            case CompressionOption.LZ4HC:
                return BuildOptions.CompressWithLz4HC;
            case CompressionOption.None:
            default:
                return BuildOptions.None;
        }
    }

    private static void ConfigureCompression(BuildTarget buildTarget)
    {
        string platformSettings = buildTarget == BuildTarget.StandaloneOSX ? "OSX" : "Standalone";
        string compressionValue = defaultCompression switch
        {
            CompressionOption.LZ4 => "LZ4",
            CompressionOption.LZ4HC => "LZ4HC",
            _ => "None"
        };

        EditorUserBuildSettings.SetPlatformSettings(platformSettings, "Compression", compressionValue);
    }

    private static string GetBuildSize(string buildPath)
    {
        if (!Directory.Exists(buildPath)) return "N/A";

        try
        {
            long totalSize = 0;
            DirectoryInfo buildInfo = new DirectoryInfo(buildPath);

            FileInfo[] allFiles = buildInfo.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo file in allFiles)
            {
                totalSize += file.Length;
            }

            return $"{totalSize / 1048576.0:F2} Mb";
        }
        catch (System.Exception exc)
        {
            Debug.LogWarning($"Failed to calculate build size: {exc.Message}");
            return "N/A";
        }
    }
}
