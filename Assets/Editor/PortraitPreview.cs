using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PortraitPreview
{
    static PortraitPreview()
    {
        EditorApplication.update += SetPreviewOnce;
    }

    static void SetPreviewOnce()
    {
        if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        EditorApplication.update -= SetPreviewOnce;
        if (SessionState.GetBool("PinballPortraitPreview400x600", false))
        {
            return;
        }

        SetPortraitPreview();
    }

    [MenuItem("Pinball/Set Portrait Preview")]
    public static void SetPortraitPreview()
    {
        Assembly editorAssembly = typeof(Editor).Assembly;
        Type sizesType = editorAssembly.GetType("UnityEditor.GameViewSizes");
        Type sizeType = editorAssembly.GetType("UnityEditor.GameViewSize");
        Type sizeKindType = editorAssembly.GetType("UnityEditor.GameViewSizeType");
        Type gameViewType = editorAssembly.GetType("UnityEditor.GameView");
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        try
        {
            object sizes = sizesType.BaseType.GetProperty("instance", flags).GetValue(null);
            object currentGroup = sizesType.GetProperty("currentGroupType", flags).GetValue(sizes);
            object group = sizesType.GetMethod("GetGroup", flags).Invoke(sizes, new object[] { currentGroup });
            Type groupClass = group.GetType();
            int count = (int)groupClass.GetMethod("GetTotalCount", flags).Invoke(group, null);
            int portraitIndex = -1;

            for (int i = 0; i < count; i++)
            {
                object size = groupClass.GetMethod("GetGameViewSize", flags).Invoke(group, new object[] { i });
                int width = (int)sizeType.GetProperty("width", flags).GetValue(size);
                int height = (int)sizeType.GetProperty("height", flags).GetValue(size);
                object kind = sizeType.GetProperty("sizeType", flags).GetValue(size);
                if (width == 400 && height == 600 && kind.ToString() == "FixedResolution")
                {
                    portraitIndex = i;
                    break;
                }
            }

            if (portraitIndex == -1)
            {
                object fixedResolution = Enum.Parse(sizeKindType, "FixedResolution");
                ConstructorInfo constructor = sizeType.GetConstructor(flags, null,
                    new Type[] { sizeKindType, typeof(int), typeof(int), typeof(string) }, null);
                object portraitSize = constructor.Invoke(new object[] { fixedResolution, 400, 600, "Pinball Portrait" });
                groupClass.GetMethod("AddCustomSize", flags).Invoke(group, new object[] { portraitSize });
                portraitIndex = count;
            }

            EditorWindow gameWindow = EditorWindow.GetWindow(gameViewType);
            gameViewType.GetProperty("selectedSizeIndex", flags).SetValue(gameWindow, portraitIndex);
            gameWindow.Repaint();
            SessionState.SetBool("PinballPortraitPreview400x600", true);
        }
        catch (Exception error)
        {
            Debug.LogError("Could not set portrait preview. Choose 400 x 600 in the Game window. " + error.Message);
        }
    }
}
