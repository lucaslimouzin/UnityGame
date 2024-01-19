using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class NavigationDisabler : EditorWindow
{
    [MenuItem("Tools/Disable All Button Navigations")]
    public static void DisableNavigation()
    {
        foreach (Button button in Resources.FindObjectsOfTypeAll(typeof(Button)))
        {
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;
            EditorUtility.SetDirty(button);
        }
    }
}
