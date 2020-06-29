using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;


[CustomEditor(typeof(ParabolaEditor))]
public class ParabolaTool : Editor
{

    [MenuItem("Tools/Parabola")]
    static void MakeParampola()
    {
        if (MakeEnv())
        {
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Parabola/Editor/Parabola.prefab");
            var go = GameObject.Instantiate(obj) as GameObject;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            Selection.activeGameObject = go;

            var camera = Camera.main;
            if (camera)
            {
                camera.transform.position = new Vector3(236, 73, -89);
                camera.transform.localEulerAngles = new Vector3(-0.6f, -61, 0);
            }
        }
    }

    public static bool MakeEnv()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return false;
        }
        else
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            GraphicsSettings.renderPipelineAsset = null;
            var light = GameObject.FindObjectOfType<Light>();
            light.transform.localEulerAngles = new Vector3(100, 0, 0);
            return true;
        }
    }

    ParabolaEditor parabola;

    private void OnEnable()
    {
        parabola = target as ParabolaEditor;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("其他程序生成信息");
        double a = parabola.RuntimeParabola(out var dir, out float speed);
        EditorGUILayout.LabelField("水平分速度:", speed.ToString("f2"));
        EditorGUILayout.LabelField("垂直加速度:", a.ToString("f2"));
        EditorGUILayout.Space();
        var ori = parabola.original.transform.position;
        var des = parabola.destination.transform.position;
        var start = new Vector2(ori.x, ori.z);
        var end = new Vector2(des.x, des.z);
        float dist = Vector2.Distance(start, end);
        EditorGUILayout.LabelField("总距离:", Vector3.Distance(des, ori).ToString("f2"));
        EditorGUILayout.LabelField("水平方向距离:", dist.ToString("f2"));
        EditorGUILayout.LabelField("垂直方向距离:", (ori.y - des.y).ToString("f2"));

        var go = parabola.go;
        if (go != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("当前位置:", go.transform.position.ToString());
            Repaint();
        }
    }

}


