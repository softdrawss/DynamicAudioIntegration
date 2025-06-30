using UnityEditor;

[InitializeOnLoad]
public static class AudioSystemManagerEditorWindowRefresh
{
    static AudioSystemManagerEditorWindowRefresh()
    {
        Selection.selectionChanged += () =>
        {
            var window = EditorWindow.GetWindow<AudioSystemManagerEditorWindow>();
            if (window != null)
                window.Repaint();
        };
    }
}