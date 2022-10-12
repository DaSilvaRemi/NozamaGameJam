using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;
using Random = UnityEngine.Random;

public delegate float EasingFuncDelegate(float t);

public class Tools
{
    /// <summary>
    /// The game state enum
    /// </summary>
    public enum GameState
    {
        MENU,
        PLAY,
        PAUSE,
        WIN,
        GAMEOVER,
        ENDLVL
    }

    /// <summary>
    /// the game scene enum
    /// </summary>
    public enum GameScene
    {
        MENUSCENE = 0, 
        FIRSTLEVELSCENE,
        HELPSCENE, 
        CREDITSCENE,
        VICTORYSCENE
    }

    public enum MoveDirection 
    { 
        LEFT, 
        RIGHT, 
        UP, 
        DOWN, 
        FORWARD, 
        BACK, 
        NONE 
    };

    /// <summary>
    /// Log an value
    /// </summary>
    /// <param name="component">The component where the message is sent</param>
    /// <param name="msg">The message</param>
    public static void Log(Component component, string msg)
    {
        Debug.Log(Time.frameCount + " " + component.GetType().Name + " " + msg);
    }

    /// <summary>
    /// Log an warning value
    /// </summary>
    /// <param name="component">The component where the message is sent</param>
    /// <param name="msg">The message</param>
    public static void LogWarning(Component component, string msg)
    {
        Debug.LogWarning(Time.frameCount + " " + component.GetType().Name + " " + msg);
    }

    /// <summary>
    /// Log an error value
    /// </summary>
    /// <param name="component">The component where the message is sent</param>
    /// <param name="msg">The message</param>
    public static void LogError(Component component, string msg)
    {
        Debug.LogError(Time.frameCount + " " + component.GetType().Name + " " + msg);
    }

    /// <summary>
    /// Get the rounded float
    /// </summary>
    /// <param name="number">The number</param>
    /// <returns>float rounded</returns>
    public static float GetRoundedFloat(float number)
    {
        return Tools.GetRoundedFloat(number, 1);
    }


    /// <summary>
    /// Get the rounded float with a precision
    /// </summary>
    /// <param name="number">The number</param>
    /// <param name="precision">The precision</param>
    /// <returns>float rounded</returns>
    public static float GetRoundedFloat(float number, int precision)
    {
        return MathF.Round(number, precision, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Format a float number to string
    /// </summary>
    /// <param name="number">the snumber</param>
    /// <returns>The number formated</returns>
    public static string FormatFloatNumberToString(float number)
    {
        return number.ToString("N01");
    }

    /**
    * <summary>Wait a time</summary> 
    * <param name="time">The time</param>
    * <returns>The IEnumerator</returns>
    */
    public static IEnumerator MyWaitCoroutine(float time, Action startAction = null, Action endAction = null)
    {
        startAction?.Invoke();
        yield return new WaitForSeconds(time);
        endAction?.Invoke();
    }

    /// <summary>
    /// An couroutine translate an object
    /// </summary>
    /// <param name="transform">The transform of the object to translate</param>
    /// <param name="startPos">The start position</param>
    /// <param name="endPos">The end position</param>
    /// <param name="duration">The current duration</param>
    /// <param name="easingFuncDelegate">The function delegate</param>
    /// <param name="speed">The speed</param>
    /// <param name="startAction">The first action to do</param>
    /// <param name="endAction">The end action</param>
    /// <returns></returns>
    public static IEnumerator MyTranslateCoroutine(Transform transform, Vector3 startPos, Vector3 endPos, float duration,
       EasingFuncDelegate easingFuncDelegate, float speed, Action startAction = null, Action endAction = null)
    {
        float elapsedTime = 0;

        startAction?.Invoke();

        while (elapsedTime < duration)
        {
            float k = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, easingFuncDelegate(speed * k));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        endAction?.Invoke();
    }

    /// <summary>
    /// A coroutine to do some action in limited time
    /// </summary>
    /// <param name="duration">The duration</param>
    /// <param name="startAction">The first action to do</param>
    /// <param name="duringAction">The action will do current time we run</param>
    /// <param name="endAction">The action will do after the time was done</param>
    /// <returns></returns>
    public static IEnumerator MyActionTimedCoroutine(float duration, Action startAction = null, Action duringAction = null, Action endAction = null)
    {
        float elapsedTime = 0;

        startAction?.Invoke();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            duringAction?.Invoke();
            yield return null;
        }

        endAction?.Invoke();
    }

    /// <summary>
    /// Set an random color
    /// </summary>
    /// <param name="gameObject">The gameobject</param>
    public static void SetRandomColor(GameObject gameObject)
    {
        MeshRenderer[] mr = gameObject.GetComponentsInChildren<MeshRenderer>();

        if (mr != null && mr.Length > 0)
        {
            for (int i = 0; i < mr.Length; i++)
            {
                Tools.SetColor(mr[i], Random.ColorHSV());
            }
        }
    }

    /// <summary>
    /// Set a color to the mesh
    /// </summary>
    /// <param name="meshRenderer">The mesh renderer</param>
    /// <param name="color">The color</param>
    public static void SetColor(MeshRenderer meshRenderer, Color color)
    {
        if (meshRenderer != null && color != null)
        {
            meshRenderer.material.color = color;
        }
    }

    /// <summary>
    /// Set a color to the mesh
    /// </summary>
    /// <param name="meshRenderer">The mesh renderer</param>
    /// <param name="color">The color</param>
    public static void SetColor(SkinnedMeshRenderer skinnedMeshRenderer, Color color)
    {
        if (skinnedMeshRenderer != null && color != null)
        {
            skinnedMeshRenderer.material.color = color;
        }
    }
}