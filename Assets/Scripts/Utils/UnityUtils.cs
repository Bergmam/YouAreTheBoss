using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityUtils
{

    static Dictionary<string, string> buttonNameDict = new Dictionary<string, string>(){
        {"Controller1Button0", "c1A"},
        {"Controller1Button1", "c1B"},
        {"Controller1Button2", "c1X"},
        {"Controller1Button3", "c1Y"},
        {"Controller1Button4", "c1LT"},
        {"Controller1Button5", "c1RT"},
        {"Controller2Button0", "c2A"},
        {"Controller2Button1", "c2B"},
        {"Controller2Button2", "c2X"},
        {"Controller2Button3", "c2Y"},
        {"Controller2Button4", "c2LT"},
        {"Controller2Button5", "c2RT"}
    };

    /// <summary>
    /// Recursively finds a child of the current transform with the specified name.
    /// </summary>
    /// <param name="current">The transform from where to start</param>
    /// <param name="name">The name of the child to look for</param>
    /// <returns>The child if found, otherwise null.</returns>
    public static Transform RecursiveFind(Transform current, string name)
    {
        if (current.name == name)
            return current;
        for (int i = 0; i < current.childCount; ++i)
        {
            Transform found = RecursiveFind(current.GetChild(i), name);
            if (found != null)
                return found;
        }
        return null;
    }

    /// <summary>
    /// Recursively gets a list of all the children of the current transform whos name contains the specified string
    /// </summary>
    /// <param name="current">The transform from where to start</param>
    /// <param name="name">The name of the child to look for</param>
    /// <returns>The child if found, otherwise null.</returns>
    public static List<GameObject> RecursiveContains(Transform current, string name)
    {
        List<GameObject> children = new List<GameObject>();

        foreach(Transform child in current.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.Contains(name))
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }

    public static bool AnyInputUp(){
        	bool vertical1Up = Input.GetAxisRaw("Vertical") > 0;
			bool vertical2Up = Input.GetAxisRaw("Vertical2") > 0;
			bool verticalControllerUp = Input.GetAxisRaw("VerticalJoystick") < 0;
			bool verticalController2Up = Input.GetAxisRaw("VerticalJoystick2") < 0;

            return vertical1Up || vertical2Up || verticalController2Up || verticalControllerUp;
    }

    public static bool AnyInputDown() {
        bool vertical1Down = Input.GetAxisRaw("Vertical") < 0;
        bool vertical2Down = Input.GetAxisRaw("Vertical2") < 0;
        bool verticalControllerDown = Input.GetAxisRaw("VerticalJoystick") > 0;
        bool verticalController2Down = Input.GetAxisRaw("VerticalJoystick2") > 0;

        return vertical1Down || vertical2Down || verticalController2Down || verticalControllerDown;
    }

    public static bool AnyInputLeft() {
        bool horizontal1Left = Input.GetAxisRaw("Horizontal") < 0;
        bool horizontal2Left = Input.GetAxisRaw("Horizontal2") < 0;
        bool verticalControllerLeft = Input.GetAxisRaw("HorizontalJoystick") < 0;
        bool verticalController2Left = Input.GetAxisRaw("HorizontalJoystick2") < 0;

        return horizontal1Left || horizontal2Left || verticalControllerLeft || verticalController2Left;
    }

    public static bool AnyInputRight() {
        bool horizontal1Right = Input.GetAxisRaw("Horizontal") > 0;
        bool horizontal2Right = Input.GetAxisRaw("Horizontal2") > 0;
        bool verticalControllerRight = Input.GetAxisRaw("HorizontalJoystick") > 0;
        bool verticalController2Right = Input.GetAxisRaw("HorizontalJoystick2") > 0;

        return horizontal1Right || horizontal2Right || verticalControllerRight || verticalController2Right;
    }

    public static string ControllerButtonToDisplayName(string button) {
        return buttonNameDict[button];
    }

    public static IEnumerator ChangeToColorAfterTime(SpriteRenderer renderer, Color color, float time) {
        yield return new WaitForSeconds(time);
		renderer.color = color;
    }
    
    public static IEnumerator ChangeToColorAfterTime(Image image, Color color, float time) {
        yield return new WaitForSeconds(time);
		image.color = color;
    }
    
    public static IEnumerator DeactiveGameObjectAfterTime(GameObject gameObject, float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    
    public static IEnumerator ActiveGameObjectAfterTime(GameObject gameObject, float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
    }
    
    public static IEnumerator ChangeToDefaultColorAfterTime(ColorModifier colorModifier, float time) {
        yield return new WaitForSeconds(time);
        if (colorModifier.gameObject.activeSelf)
        {
            colorModifier.DeSelect();
        }
    }
}
