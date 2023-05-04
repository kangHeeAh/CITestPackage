#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PackageExporter
{
  private static string originalPackageText;
  
  [MenuItem("Tools/Export Unitypackage")]
  public static void Export()
  {
    var packageRoot = Path.Combine(Application.dataPath, "..", "Packages", "com.example.example_gameci");    

    // var version = GetVersion(packageRoot);
    // var fileName = string.IsNullOrEmpty(version) ? "ExampleGameCI.unitypackage" : $"ExampleGameCI.{version}.unitypackage";
    var fileName = "ExampleGameCI.unitypackage";
    var exportPath = "./package/" + fileName;

    var pluginAssets = EnumerateAssets(Path.Combine("Packages", "com.example.example_gameci")); // export all the files
    var sampleAssets = EnumerateAssets(Path.Combine("Assets", "Samples")); // export all the files
    var assets = pluginAssets.Concat(sampleAssets).ToArray();

    Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

    AssetDatabase.ExportPackage(
        assets,
        exportPath );

    Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
  }

  private static IEnumerable<string> EnumerateAssets(string path)
  {
    var projectRoot = Path.Combine(Application.dataPath, "..");
    var assetRoot = Path.Combine(projectRoot, path);

    return Directory.EnumerateFiles(assetRoot, "*", SearchOption.AllDirectories)
        .Select(x => path + x.Replace(assetRoot, "").Replace(@"\", "/"));
  }

  private static IEnumerable<string> EnumerateAssets(string path, string[] extensions)
  {
    return EnumerateAssets(path).Where(x => Array.IndexOf(extensions, Path.GetExtension(x)) >= 0);
  }

  private static void ChangePackageSample(string packagePath)
  {
    var packageJsonPath = Path.Combine(packagePath, "package.json");
    if (File.Exists(packageJsonPath))
    {
        originalPackageText = File.ReadAllText(packageJsonPath); 

        var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(packageJsonPath)); // 이러면 dependencies가 포함되지 않는다.
        packageJson.samples = null; // 샘플 초기화하기
        File.WriteAllText(packageJsonPath, JsonUtility.ToJson(packageJson));
    }
  }

  private static void SetOriginalPackageSample(string packagePath)
  {
    var packageJsonPath = Path.Combine(packagePath, "package.json");
    {
        File.WriteAllText(packageJsonPath, originalPackageText);
    }
  }

  private static string GetVersion(string packagePath)
  {
    var version = Environment.GetEnvironmentVariable("UNITY_PACKAGE_VERSION");
    var packageJsonPath = Path.Combine(packagePath, "package.json");

    if (File.Exists(packageJsonPath))
    {
      var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(packageJsonPath));

      if (!string.IsNullOrEmpty(version))
      {
        if (packageJson.version != version)
        {
          var msg = $"package.json and env version are mismatched. UNITY_PACKAGE_VERSION:{version}, package.json:{packageJson.version}";

          if (Application.isBatchMode)
          {
            Console.WriteLine(msg);
            Application.Quit(1);
          }

          throw new Exception("package.json and env version are mismatched.");
        }
      }

      version = packageJson.version;
    }

    return version;
  }

  public class PackageJson
  {
    public string name;
    public string version;
    public string displayName;
    public string description;
    // public string unity;
    // public Author author;
    // public string changelogUrl;
    public Dictionary<string, string> dependencies;
    // public string documentationUrl;
    // public bool hideInEditor;
    // public List<string> keywords;
    // public string license;
    // public string licenseUrl;
    public List<Sample> samples;
    // public string type;
    // public string unityRelease;

    [Serializable]
    public class Author
    {
      public string name;
      public string email;
      public string url;
    }

    [Serializable]
    public class Sample
    {
      public string displayName;
      public string description;
      public string path;
    }
  }
}

#endif