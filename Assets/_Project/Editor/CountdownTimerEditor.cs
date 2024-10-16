#region
using UnityEditor;
using UnityEngine;
#endregion

[CustomEditor(typeof(CountdownTimer))]
public class CountdownTimerEditor : Editor
{
    float newTime;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var timer = (CountdownTimer) target;

        GUILayout.Space(20);

        GUILayout.Label("Timer Controls", EditorStyles.largeLabel, GUILayout.Height(20));

        using (new GUILayout.HorizontalScope())
        {
            newTime = EditorGUILayout.FloatField("Set Time", newTime, GUILayout.Height(20)); // put this line where you want the input field to be

            if (GUILayout.Button("Set Time", GUILayout.Height(20))) timer.SetTimer(newTime);
        }

        if (GUILayout.Button("Reset Timer", GUILayout.Height(20))) timer.ResetTimer();
    }
}
