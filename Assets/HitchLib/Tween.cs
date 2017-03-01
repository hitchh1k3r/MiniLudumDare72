using UnityEngine;
using System.Collections;

namespace HitchLib
{

  // NOTE (hitch) forked from code on 2-26-17 :: so far this code is not part of a central library,
  // I susspect it will grow here and eventually become the central version. If not hopefully it's
  // changes can be merged up stream.

  // HitchLib //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//
  public static class Tweening
  {

    public static IEnumerator GenericTween<T>(System.Action<T> setOutput, T start, T end, float length, Easing.EaseMethod ease) where T: new()
    {
      float track = 0;
      T ret = start;
      while(track < length)
      {
        float t = ease(track / length);

        // NOTE (hitch) 2-19-17 In C# 4.0 we could cast `start` and `end` to `dynamic` rather than
        // this ugly casting nightmare that only supports types explicitly set here:
        // - Some types might still need special handeling (ex. Quaternions).
        // - Also if we did this we could move from `ILerpable` to implementing * operator
        //   overloading!
        if(typeof(T) == typeof(Vector3))
        {
          ret = (T)(object)((1-t)*((Vector3)(object)start) + t * ((Vector3)(object)end));
        }
        else if(typeof(T) == typeof(Color))
        {
          ret = (T)(object)((1-t)*((Color)(object)start) + t * ((Color)(object)end));
        }
        else if(typeof(T) == typeof(float))
        {
          ret = (T)(object)((1-t)*((float)(object)start) + t * ((float)(object)end));
        }
        else if(typeof(T) == typeof(double))
        {
          ret = (T)(object)((1-t)*((double)(object)start) + t * ((double)(object)end));
        }
        else if(typeof(T) == typeof(Vector2))
        {
          ret = (T)(object)((1-t)*((Vector2)(object)start) + t * ((Vector2)(object)end));
        }
        else if(typeof(T) == typeof(Vector4))
        {
          ret = (T)(object)((1-t)*((Vector4)(object)start) + t * ((Vector4)(object)end));
        }
        else if(typeof(T) == typeof(Quaternion))
        {
          ret = (T)(object)Quaternion.SlerpUnclamped((Quaternion)(object)start,
                (Quaternion)(object)end, t);
        }

        setOutput(ret);
        yield return null;
        track += Time.deltaTime;
      }
      setOutput(end);
    }

    public static IEnumerator GenericTweenLerpable<T>(System.Action<T> setOutput, T start, T end, float length, Easing.EaseMethod ease) where T: ILerpable<T>
    {
      float track = 0;
      while(track < length)
      {
        float t = ease(track / length);

        setOutput(start.LerpToward(end, t));
        yield return null;
        track += Time.deltaTime;
      }
      setOutput(end);
    }

    public static IEnumerator FadeTo(UnityEngine.UI.Graphic uiElement, float length, Easing.EaseMethod ease, float alpha = 0)
    {
      float track = 0;
      Color start = uiElement.color;
      Color end = start;
      end.a = alpha;
      while(track < length)
      {
        float t = ease(track / length);

        uiElement.color = ((1-t)*start + t * end);
        yield return null;
        track += Time.deltaTime;
      }
      uiElement.color = end;
    }

    // HitchLib // Tweening //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*
    public interface ILerpable<T> where T: ILerpable<T>
    {

      T LerpToward(T end, float t);

    }

  }

  // HitchLib //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//
  public static class Easing
  {

    public delegate float EaseMethod(float f);

    public static readonly EaseMethod EASE_LINEAR       = (t => t);
    public static readonly EaseMethod EASE_QUAD_IN      = (t => t*t);
    public static readonly EaseMethod EASE_QUAD_OUT     = (t => t * (2-t));
    public static readonly EaseMethod EASE_QUAD_IN_OUT  = EaseInOut(EASE_QUAD_IN, EASE_QUAD_OUT);
    public static readonly EaseMethod EASE_CUBIC_IN     = (t => t*t*t);
    public static readonly EaseMethod EASE_CUBIC_OUT    = (t => { --t; return (t*t*t + 1); });
    public static readonly EaseMethod EASE_CUBIC_IN_OUT = EaseInOut(EASE_CUBIC_IN, EASE_CUBIC_OUT);
    public static readonly EaseMethod EASE_SIN_IN       = (t => 1 - Mathf.Cos((Mathf.PI/2)*t));
    public static readonly EaseMethod EASE_SIN_OUT      = (t => Mathf.Sin((Mathf.PI/2)*t));
    public static readonly EaseMethod EASE_SIN_IN_OUT   = EaseInOut(EASE_SIN_IN, EASE_SIN_OUT);
    public static readonly EaseMethod EASE_EXP_IN       = (t => Mathf.Pow(2, 10*(t-1)));
    public static readonly EaseMethod EASE_EXP_OUT      = (t => -Mathf.Pow(2,-10*t) + 1 );
    public static readonly EaseMethod EASE_EXP_IN_OUT   = EaseInOut(EASE_EXP_IN, EASE_EXP_OUT);
    public static readonly EaseMethod EASE_CIRC_IN      = (t => 1 - Mathf.Sqrt(1 - t*t));
    public static readonly EaseMethod EASE_CIRC_OUT     = (t => { --t; return Mathf.Sqrt(1 - t*t); });
    public static readonly EaseMethod EASE_CIRC_IN_OUT  = EaseInOut(EASE_CIRC_IN, EASE_CIRC_OUT);
    public static readonly EaseMethod EASE_ELASTIC_OUT  = EaseElastic(3.33333333333f);

    public static EaseMethod EaseInOut(EaseMethod easeIn, EaseMethod easeOut)
    {
      return (t => ((t < 0.5f) ? (0.5f * easeIn(2*t)) : (0.5f * easeOut(2*t - 1) + 0.5f)));
    }

    public static EaseMethod EaseElastic(float bounces)
    {
      return (t => (Mathf.Pow(2, (-10*t)) * Mathf.Sin((t-1/(4*bounces))*(2*Mathf.PI)*bounces) + 1));
    }

    public static EaseMethod EaseInvert(EaseMethod ease)
    {
      return (t => 1 - ease(1 - t));
    }

  }

}
