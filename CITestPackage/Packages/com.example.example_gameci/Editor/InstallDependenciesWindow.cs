#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using System.Collections;

public static class DependenciesWindowInstaller
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        Events.registeredPackages += OnPostProcess;
    }

    private static void OnPostProcess(PackageRegistrationEventArgs args)
    {
        foreach(var packageInfo in args.added)
        {
            if(packageInfo.name.Equals("com.example.example_gameci"))
            {
                InstallDependenciesWindow window = (InstallDependenciesWindow)EditorWindow.GetWindow(typeof(InstallDependenciesWindow));
                window.Show();
                break;
            }
        }
    }
}

public static class InstallDependen
{
    [MenuItem("Tools/Install Dependen")]
    public static void Install()
    {
        InstallDependenciesWindow window = (InstallDependenciesWindow)EditorWindow.GetWindow(typeof(InstallDependenciesWindow));
        window.Show();
    }
}

public class InstallDependenciesWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Label("Welcome to Example GameCI!");

        if (GUILayout.Button("Install Dependencies"))
        {
            var request = Client.Add("https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.107.2");
            while (!request.IsCompleted) {}
            request = Client.Add("https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.107.2");
            while (!request.IsCompleted) {}
            request = Client.Add("https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM#v0.107.2");
            while (!request.IsCompleted) {}
            request = Client.Add("https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.107.2");
            while (!request.IsCompleted) {}
        }
    }
}

#endif