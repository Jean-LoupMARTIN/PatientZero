using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomExtension
{
    public static T PickRandom<T>(this T[] array) => array.Length == 0 ? default : array[(int)(Random.value * array.Length)];
    public static T PickRandom<T>(this List<T> list) => list.Count == 0 ? default : list[(int)(Random.value * list.Count)];
    public static T PickRandom<T>(this IEnumerable<T> ienum) => ienum.ToArray().PickRandom();


    public static Vector2 RandomVector2(float min, float max) => new Vector2(Random.Range(min, max), Random.Range(min, max));
    public static Vector3 RandomVector3(float min, float max) => new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
}


