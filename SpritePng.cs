using System.Collections;
using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SpritePng))]
public class SpritePngEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SpritePng my = (SpritePng)target;
        if (GUILayout.Button("Open Library", GUILayout.Height(20))){my.OpenLibrary();}
        if (GUILayout.Button("ScreenShot", GUILayout.Height(50))){my.ScreenShot();}
    }
}
#endif
public class SpritePng : MonoBehaviour {


    [Range(1,8)]
    public int QualityMultiply = 1;
    Texture2D Screenshot() {
        Camera cameram = GetComponent<Camera>();

        int resWidth = cameram.pixelWidth*QualityMultiply;
        int resHeight = cameram.pixelHeight*QualityMultiply;
        Camera camera = cameram;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 32);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply ();
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        return screenShot;
     }
 
    public Texture2D SaveScreenshotToFile(string fileName){
         Texture2D screenShot = Screenshot ();
         byte[] bytes = screenShot.EncodeToPNG();
         System.IO.File.WriteAllBytes(fileName, bytes);
         return screenShot;
    }
    public string GetPatch(){
        return Application.dataPath.Replace("Assets","")+"SpritePng/";
    }
    
    // int Count = 0;
	public void ScreenShot(){
        System.IO.Directory.CreateDirectory(GetPatch());
        PlayerPrefs.SetInt("SpritePng_Count",PlayerPrefs.GetInt("SpritePng_Count")+1);
        int Count = PlayerPrefs.GetInt("SpritePng_Count");
        SaveScreenshotToFile(GetPatch()+Count+".png");
        Application.OpenURL(GetPatch());
    }
    public void OpenLibrary(){
        Application.OpenURL(GetPatch());
    }
    // public QualityEnum Qualitym;
    // public enum QualityEnum{
    //     x2,x4,x8
    // }
}
