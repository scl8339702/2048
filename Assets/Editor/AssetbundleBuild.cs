using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public class AssetbundleBuild : MonoBehaviour
{
    
    // Start is called before the first frame update
    [MenuItem("Custom Editor/Build AssetBundle")]

    static void buildAssetBundle()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/StreamingAssets", BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
