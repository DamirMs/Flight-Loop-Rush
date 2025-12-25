using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assets.Scripts.Libraries.RSG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum DirectionEnum { Left, Right, Down, Up }

public static class Utils
{
    #region Important

    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static IEnumerable<TResult> ConvertTo<TSource, TResult>(TSource[] sourceArray, Func<TSource, TResult> selector)
    {
        foreach (var item in sourceArray)
        {
            yield return selector(item);
        }
    }

    public static T Parse<T>(this object value)
    {
        if (!typeof(T).IsEnum)
            return (T)Convert.ChangeType(value, typeof(T));

        if (Enum.IsDefined(typeof(T), value))
            return (T)value;

        return default;
    }

    public static List<T> GetAllPublicStaticFieldsValues<T>(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue())
            .ToList();
    }

    public static T[] GetRange<T>(this T[] array, int index, int count)
    {
        T[] tmpArray = new T[count];
        Array.Copy(array, index, tmpArray, 0, count);
        return tmpArray;
    }

    public static void SetActive(this Component m, bool active)
    {
        if (m && m.gameObject) m.gameObject.SetActive(active);
    }
    
    public static void SetActive(this GameObject[] objects, bool active)
    {
        foreach (var obj in objects) obj.SetActive(active);
    }

    #endregion
    #region Direction 

    public static Vector2 GetNormalizedDiagonalDirection(Vector2 direction)
    {
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        float snappedAngle = Mathf.Round(angle / 45) * 45;
        if (snappedAngle == 360) snappedAngle = 0;

        return GetAngleToDirection(snappedAngle);
    }
    private static Vector2 GetAngleToDirection(float angle)
    {
        switch (angle)
        {
            case 0: return Vector2.right;
            case 45: return new Vector2(0.75f, 0.75f);
            case 90: return Vector2.up;
            case 135: return new Vector2(-0.75f, 0.75f);
            case 180: return Vector2.left;
            case 225: return new Vector2(-0.75f, -0.75f);
            case 270: return Vector2.down;
            case 315: return new Vector2(0.75f, -0.75f);
            default: DebugManager.Log(DebugCategory.Misc, "normalized angle of 8 points direction is 0"); return Vector2.up;
        }
    }

    public static void GetNormalizedCardinalDirection(this Vector2 direction)
    {
        direction.Normalize();

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector2(direction.x > 0 ? 1 : -1, 0);
        }
        else //if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            direction = new Vector2(0, direction.y > 0 ? 1 : -1);
        }
    }

    public static List<Vector2> GetCardinalDirectionsList()
        => new() { Vector2.left, Vector2.right, Vector2.down, Vector2.up };

    public static bool IsDirectionCardinal(Vector2 direction) =>
        Mathf.Abs(direction.x) == 1 || Mathf.Abs(direction.y) == 1;

    public static Vector3 NormalizeDirectionTo(this Vector3 positionFrom, Vector3 positionTo) =>
        (positionTo - positionFrom).normalized;
    public static Vector3 NormalizeDirectionTo(this Vector2 positionFrom, Vector2 positionTo) =>
        (positionTo - positionFrom).normalized;

    public static DirectionEnum GetDirectionEnumFromCardinalDirection(Vector2 direction)
    {
        switch (direction)
        {
            case Vector2 { x: -1, y: 0 }: return DirectionEnum.Left;
            case Vector2 { x: 1, y: 0 }: return DirectionEnum.Right;
            case Vector2 { x: 0, y: -1 }: return DirectionEnum.Down;
            case Vector2 { x: 0, y: 1 }: return DirectionEnum.Up;

            default: DebugManager.Log(DebugCategory.Misc, "direction was not cardinal"); return DirectionEnum.Up;
        }
    }
    public static Vector2 GetDirection(DirectionEnum directionEnum)
    {
        switch (directionEnum)
        {
            case DirectionEnum.Left: return new(-1, 0);
            case DirectionEnum.Right: return new(1, 0);
            case DirectionEnum.Down: return new(0, -1);
            case DirectionEnum.Up: return new(0, 1);
            default: return Vector2.zero;
        }
    }

    #endregion
    #region Misc

    public static void AdjustValueBetweenMinMax(int min, int max, ref int value) => value = Math.Max(min, Math.Min(value, max));
    public static int AdjustValueBetweenMinMax(int min, int max, int value) => value = Math.Max(min, Math.Min(value, max));
    public static float AdjustValueBetweenMinMax(float min, float max, float value) => value = Math.Max(min, Math.Min(value, max));

    public static bool ValueIsInBetweenValues(int start, int end, int value) => value >= start && value < end;

    public static bool ContainsPositionWithOffset(List<Vector2> positions, Vector2 targetPosition, float offset)
        => positions.Any(x => Vector2.Distance(x, targetPosition) < offset);
    public static Vector2 GetPositionCloseToOffset(List<Vector2> positions, Vector2 targetPosition, float offset)
        => positions.FirstOrDefault(x => Vector2.Distance(x, targetPosition) < offset);

    public static Vector2 GetMultipliedVector(Vector2 value, Vector2 multiplier)
        => new(value.x * multiplier.x, value.y * multiplier.y);

    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;
        deltaPosition.Scale(rectTransform.rect.size);
        deltaPosition.Scale(rectTransform.localScale);
        deltaPosition = rectTransform.rotation * deltaPosition;

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

    public static List<T> Clone<T>(this List<T> oldList)
    {
        return new List<T>(oldList);
    }

    public static Dictionary<T1, T2> Clone<T1, T2>(this Dictionary<T1, T2> oldDictionary)
    {
        return oldDictionary.ToDictionary(entry => entry.Key,
                                   entry => entry.Value);
    }

    public static T Shift<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            var result = list[0];
            list.RemoveAt(0);
            return result;
        }
        return default(T);
    }

    public static void ResolveOnce(this Promise p)
    {
        if (p.CurState == PromiseState.Pending)
            p.Resolve();
    }
    public static void RejectOnce(this Promise p, Exception e = null)
    {
        if (p.CurState == PromiseState.Pending)
            p.Reject(e);
    }

    public static string GetEnumLabel<EnumType>(EnumType feature) where EnumType : Enum
    {
        string label = feature.ToString();
        var enumType = typeof(EnumType);
        var memberInfo = enumType.GetMember(label).FirstOrDefault();
        var valueAttribute = memberInfo?.GetCustomAttributes(typeof(InspectorNameAttribute), false)
                        .FirstOrDefault() as InspectorNameAttribute;
        if (valueAttribute != null)
            label = valueAttribute.displayName;

        return label;
    }

    #endregion
    #region Random 
    public static float GetRandomValue(this Vector2 minMax) =>
        minMax.x > minMax.y? Random.Range(minMax.y, minMax.x) : Random.Range(minMax.x, minMax.y);
    public static int GetRandomValue(this Vector2Int minMax) =>
        minMax.x > minMax.y? Random.Range(minMax.y, minMax.x) : Random.Range(minMax.x, minMax.y);
    public static float GetRandomValue(float min, float max) => Random.Range(min, max);
    public static int GetRandomValueInt(int min, int max) => Random.Range(min, max);
    public static int GetRandomValueTo(int max) => Random.Range(0, max);
    public static float GetRandomNext(float max) => Random.Range(0, max);
    public static int GetRandomNextInt(int max) => Random.Range(0, max);
    public static T GetRandomEnum<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(GetRandomNextInt(values.Length));
    }
    public static T GetRandomElement<T>(this IList<T> elements) => elements[GetRandomNextInt(elements.Count)];
    public static T GetRandomElement<T>(this T[] elements) => elements[GetRandomNextInt(elements.Length)];
    public static T GetRandomElement<T>(this List<T> elements) => elements[GetRandomNextInt(elements.Count)];
    public static Vector3 GetRandomDirection()
    {
        var result = new Vector3(GetRandomValue(new Vector2(-1, 1)), GetRandomValue(new Vector2(-1, 1)), GetRandomValue(new Vector2(-1, 1)));

        return result.normalized;
    }
    public static Vector3 GetRandomBetweenVectors(Vector3 min, Vector3 max)
    {
        return new(GetRandomValue(min.x, max.x), GetRandomValue(min.y, max.y), GetRandomValue(min.z, max.z));
    }
    public static Vector2 GetRandomVector(float value) => new(GetRandomValue(-value, value), GetRandomValue(-value, value));
    
    public static T PickByWeight<T>(List<(T item, float weight)> weightedList)
    {
        float total = 0f;
        foreach (var w in weightedList) total += w.weight;

        float rand = GetRandomNext(total);
        float cumulative = 0f;
        foreach (var (item, weight) in weightedList)
        {
            cumulative += weight;
            if (rand <= cumulative) return item;
        }

        return default;
    }

    #endregion
    #region Async 

    public static IEnumerator WaitForTaskCompletion(Task task)
    {
        while (!task.IsCompleted) yield return null;
    }

    public static async Task WaitInSeconds(int value) => await Task.Delay(value * 1000);
    public static async Task WaitInSeconds(float value)
    {
        int milliseconds = Mathf.RoundToInt(value * 1000f);

        await Task.Delay(milliseconds);
    }

    private static MonoBehaviour _mb = null;
    public static void SetMainContainer(MonoBehaviour mainContainer) => _mb = mainContainer;
    public static IPromise AsyncOperationToPromise(AsyncOperation operation)
    {
        if (operation.isDone) return Promise.Resolved();
        var result = new Promise();
        _mb.StartCoroutine(coroutine(operation, result));

        IEnumerator coroutine(AsyncOperation operation, Promise promise)
        {
            while (!operation.isDone)
                yield return null;

            promise.Resolve();
        }

        return result;
    }


    public static Promise Wait(float time)
    {
        var promise = new Promise();

        if (time == 0f)
            promise.Resolve();
        else
        {
            if (_mb && _mb.gameObject.activeSelf)
                _mb.StartCoroutine(timer());
            else
                Debug.LogWarning("_mb is null or inactive");
        }

        return promise;

        IEnumerator timer()
        {
            yield return new WaitForSeconds(time);
            promise.Resolve();
        }
    }

    public static IPromise Wait(float time, GameObject link, bool ignoreTimeScale)
    {
        var promise = new Promise();

        DOVirtual.DelayedCall(time, after, ignoreTimeScale).SetLink(link);

        return promise;

        void after()
        {
            promise.Resolve();
        }
    }


    #endregion
    #region UI

    private readonly static Vector2 ReferenceResolution = new(1920, 1080);

    private static Vector2 _scaleFactor;
    private static Vector2 ScaleFactor
    {
        get
        {
            if (_scaleFactor == null || _scaleFactor == Vector2.zero)
            {
                var screenResolution = new Vector2(Screen.width, Screen.height);

                _scaleFactor = new Vector2(screenResolution.x / ReferenceResolution.x, screenResolution.y / ReferenceResolution.y);
            }

            return _scaleFactor;
        }
        set { _scaleFactor = value; }
    }


    public static void AddEventToEventTrigger(EventTrigger eventTrigger, Action action, EventTriggerType eventTriggerType)
    {
        var pointerUpEntry = new EventTrigger.Entry
        {
            eventID = eventTriggerType
        };
        pointerUpEntry.callback.AddListener(_ => action?.Invoke());
        eventTrigger.triggers.Add(pointerUpEntry);
    }

    public static void AdjustYValue(ref float value) => value *= ScaleFactor.y;
    public static void AdjustXValue(ref float value) => value *= ScaleFactor.x;
    public static void AdjustXVector(ref Vector2 position) => position.x *= ScaleFactor.x;
    public static void AdjustYVector(ref Vector2 position) => position.y *= ScaleFactor.y;

    public static void ResizeSpriteToScreen(SpriteRenderer sprite, Camera theCamera, int fitToScreenWidth = 1, int fitToScreenHeight = 1)
    {
        sprite.transform.localScale = new Vector3(1, 1, 1);

        var width = sprite.sprite.bounds.size.x;
        var height = sprite.sprite.bounds.size.y;

        var worldScreenHeight = theCamera.orthographicSize * 2.0f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        if (fitToScreenWidth != 0)
        {
            var sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth, sprite.transform.localScale.y);
            sprite.transform.localScale = sizeX;
        }

        if (fitToScreenHeight != 0)
        {
            var sizeY = new Vector2(sprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
            sprite.transform.localScale = sizeY;
        }
    }

    #endregion
    #region Animation 

    public static IPromise ToPromise(this TweenerCore<Vector3, Vector3, VectorOptions> tween)
    {
        var promise = new Promise();
        var onWasComplete = tween.onComplete;
        var onWasKill = tween.onKill;
        tween.OnComplete(() =>
        {
            onWasComplete?.Invoke();
            promise.ResolveOnce();
        });
        tween.OnKill(() =>
        {
            onWasKill?.Invoke();
            promise.RejectOnce();
        });
        return promise;
    }

    public static IPromise ToPromise(this TweenerCore<float, float, FloatOptions> tween)
    {
        var promise = new Promise();
        tween.OnComplete(promise.ResolveOnce);
        tween.OnKill(() => promise.RejectOnce());
        return promise;
    }

    public static IPromise ToPromise(this Tween tween)
    {
        var promise = new Promise();
        var onWasComplete = tween.onComplete;
        var onWasKill = tween.onKill;
        tween.OnComplete(() =>
        {
            onWasComplete?.Invoke();
            promise.ResolveOnce();
        });
        tween.OnKill(() =>
        {
            onWasKill?.Invoke();
            promise.RejectOnce();
        });
        return promise;
    }

    public static IPromise ToPromise(this Sequence sequence)
    {
        var promise = new Promise();
        var onWasComplete = sequence.onComplete;
        var onWasKill = sequence.onKill;
        sequence.OnComplete(() =>
        {
            onWasComplete?.Invoke();
            promise.ResolveOnce();
        });
        sequence.OnKill(() =>
        {
            onWasKill?.Invoke();
            promise.RejectOnce();
        });
        return promise;
    }

    public static IPromise ToPromise(this TweenerCore<Quaternion, Vector3, QuaternionOptions> tween)
    {
        var promise = new Promise();
        tween.OnComplete(promise.Resolve);
        return promise;
    }

    #endregion
    #region Formatting

    public static T ReadFile<T>(string fileName)
    {
        string destination = Application.persistentDataPath + "/" + fileName;
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.LogWarning("[ReadFile] " + fileName + " not found");
            return default(T);
        }

        BinaryFormatter bf = new BinaryFormatter();

        object data = bf.Deserialize(file);
        file.Close();

        return (T)data;
    }

    public static void SaveFile(string fileName, object data)
    {
        string destination = Application.persistentDataPath + "/" + fileName;
        FileStream file;

        Debug.LogWarning($"Save file {destination}");

        if (File.Exists(destination))
            file = File.OpenWrite(destination);
        else
            file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static void RemoveJson(string fileName)
    {
        RemoveFile(fileName + ".json");
    }

    public static void RemoveFile(string fileName)
    {
        string destination = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(destination))
            File.Delete(destination);
    }

    public static bool CloneJson(string from, string to)
    {
        return CloneFile(from + ".json", to + ".json");
    }

    public static bool CloneFile(string from, string to)
    {
        var fromDist = Application.persistentDataPath + "/" + from;
        var toDist = Application.persistentDataPath + "/" + to;
        if (!File.Exists(fromDist))
            return false;

        File.Copy(Application.persistentDataPath + "/" + from,
            Application.persistentDataPath + "/" + to);
        return true;
    }

    public static bool IsJsonExists(string fileName)
    {
        return IsFileExists(fileName + ".json");
    }

    public static bool IsFileExists(string fileName)
    {
        string destination = Application.persistentDataPath + "/" + fileName;

        return File.Exists(destination);
    }

    #endregion
    #region Server Related

    private static readonly char[] CharsToEscape = { '!', '*', '\'', '(', ')' };

    public static object AnimationControllerFactory { get; internal set; }

    /// <summary>
    /// Escapes a string according to the URL data string rules given in RFC 3986.
    /// </summary>
    /// <param name="value">The value to escape.</param>
    /// <returns>The escaped value.</returns>
    /// <remarks>
    /// The <see cref="Uri.EscapeDataString"/> method is <i>supposed</i> to take on
    /// RFC 3986 behavior if certain elements are present in a .config file.  Even if this
    /// actually worked (which in my experiments it <i>doesn't</i>), we can't rely on every
    /// host actually having this configuration element present.
    /// </remarks>
    public static string Escape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        // Start with RFC 2396 escaping by calling the .NET method to do the work.
        // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
        // If it does, the escaping we do that follows it will be a no-op since the
        // characters we search for to replace can't possibly exist in the string.
        var lEscaped = new StringBuilder(Uri.EscapeDataString(value));

        // Upgrade the escaping to RFC 3986, if necessary.
        for (int i = 0; i < CharsToEscape.Length; i++)
            lEscaped.Replace(char.ToString(CharsToEscape[i]), Uri.HexEscape(CharsToEscape[i]));

        // Return the fully-RFC3986-escaped string.
        return lEscaped.ToString();
    }

    #endregion
    #region Extentions
    #region Math
    public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
        => (1 - t) * (1 - t) * Start + 2 * t * (1 - t) * Control + t * t * End;
    public static Vector3 Bezier3(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float t)
    {
        var tm = 1 - t;
        return tm * tm * tm * start + 3 * t * tm * tm * control1 + 3 * t * t * tm * control2 + t * t * t * end;
    }

    public static T Clamp<T>(this T target, T min, T max) where T : IComparable
    {
        if (target.CompareTo(min) < 0) return min;
        if (target.CompareTo(max) > 0) return max;
        return target;
    }
    #endregion
    #region Vector
    public static Vector2Int GetDelta(this Vector2Int vect, int dx, int dy) => new (vect.x + dx, vect.y + dy);
    public static Vector2Int GetDelta(this Vector2Int vect, Vector2Int d) => GetDelta(vect, d.x, d.y);

    public static Vector3 toVector3(this Vector2 vec) => new (vec.x, vec.y);
    public static Vector3 ToVector3(this Vector2Int vec, int z = 0) => new (vec.x, vec.y, z);
    public static Vector3 toVector3(this float f) => new (f, f, f);
    public static Vector3Int ToVector3Int(this Vector2Int vec, int z = 0) => new (vec.x, vec.y, z);
    public static Vector2 toVector2(this Vector3 vec) => new (vec.x, vec.y);

    public static Vector3 Clamp(this Vector3 vector, Vector3 minBounds, Vector3 maxBounds)
    {
        var x = Mathf.Clamp(vector.x, minBounds.x, maxBounds.x);
        var y = Mathf.Clamp(vector.y, minBounds.y, maxBounds.y);
        var z = Mathf.Clamp(vector.z, minBounds.z, maxBounds.z);
        return new Vector3(x, y, z);
    }
    public static Vector2 Clamp(this Vector2 vector, Vector2 minBounds, Vector2 maxBounds)
    {
        var x = Mathf.Clamp(vector.x, minBounds.x, maxBounds.x);
        var y = Mathf.Clamp(vector.y, minBounds.y, maxBounds.y);
        return new Vector2(x, y);
    }

    public static Vector3 Set(this Vector3 v, float? x = null, float? y = null, float? z = null) =>
            new (x ?? v.x, y ?? v.y, z ?? v.z);
    public static Vector2 Set(this Vector2 v, float? x = null, float? y = null) =>
        new (x ?? v.x, y ?? v.y);

    public static Vector3 Add(this Vector3 v, float? x = null, float? y = null, float? z = null) =>
            new (v.x + (x ?? 0), v.y + (y ?? 0), v.z + (z ?? 0));
    public static Vector2 Add(this Vector2 v, float? x = null, float? y = null) =>
        new (v.x + (x ?? 0), v.y + (y ?? 0));

    public static Vector2 MultiplyElementWise(this Vector2 v, float? x = null, float? y = null, float? z = null) =>
        new(x == null ? v.x : v.x * x.Value,
            y == null ? v.y : v.y * y.Value);
    public static Vector3 MultiplyElementWise(this Vector3 v, float? x = null, float? y = null, float? z = null) =>
        new(x == null ? v.x : v.x * x.Value,
            y == null ? v.y : v.y * y.Value,
            z == null ? v.z : v.z * z.Value);
    #endregion
    #region Color
    public static Color Set(this Color col, float? r = null, float? g = null, float? b = null, float? a = null) =>
            new(r ?? col.r, g ?? col.g, b ?? col.b, a ?? col.a);
    public static Color SetAlpha(this Color color, float alpha) =>
        new(color.r, color.g, color.b, alpha);

    public static Image SetColor(this Image i, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        i.color = i.color.Set(r, g, b, a);
        return i;
    }
    public static TextMeshProUGUI SetColor(this TextMeshProUGUI i, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        i.color = i.color.Set(r, g, b, a);
        return i;
    }
    public static SpriteRenderer SetColor(this SpriteRenderer i, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        i.color = i.color.Set(r, g, b, a);
        return i;
    }

    public static Color NumberToColor(this int number) => new((number >> 24) % 256, (number >> 16) % 256, (number >> 8) % 256, number % 256);
    public static Color HexToColor(string hexColor) => Convert.ToInt32(hexColor, 16).NumberToColor();
    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
    #endregion

    public static void AddSingle<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }

    public static void DestroyAllChildren(this Transform transform, params Transform[] exclusions)
    {
        transform.GetChildren().Where(child => exclusions.Contains(child) == false).ToList().ForEach(child => GameObject.Destroy(child.gameObject));
    }

    #region Unity hierarchy utils (myb remove)
    public static List<(GameObject, MatchCollection)> FindInChildrenByName(this Transform t, string regularEx, bool recursive = false)
    {
        var regEx = new Regex(regularEx);

        var result = new List<(GameObject, MatchCollection)>();

        for (var i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i);
            if (!child) continue;

            if (regEx.IsMatch(child.name))
            {
                result.Add((child.gameObject, regEx.Matches(child.name)));
            }

            if (recursive)
                result.AddRange(child.FindInChildrenByName(regularEx, true));
        }

        return result;
    }

    public static Transform FindInChildrenRecursively(this Transform t, string name)
    {
        var queue = new Queue<Transform>();
        queue.Enqueue(t);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var res = current.Find(name);

            if (res) return res;

            current.ForeachChild(x => queue.Enqueue(x));
        }

        return null;
    }

    public static List<(GameObject, Match)> FindAllInChildrenRecursivelyByRegEx(this Transform t, string regularEx,
        Func<string, string> nameChanger = null)
    {
        var regEx = new Regex(regularEx);

        var result = new List<(GameObject, Match)>();

        t.ForeachChildrenRecursively(child =>
        {
            if (!child) return;

            var childName = nameChanger?.Invoke(child.name) ?? child.name;
            var match = regEx.Match(childName);
            if (match.Success) result.Add((child.gameObject, match));
        });

        return result;
    }

    public static T GetComponentInParents<T>(this GameObject gameObject)
    {
        return GetComponentInParents<T>(gameObject.transform);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject) return null;

        var t = gameObject.GetComponent<T>();
        if (!t) t = gameObject.AddComponent<T>();
        return t;
    }


    public static T GetComponentInParents<T>(this Transform start)
    {
        while (start != null)
        {
            var comp = start.GetComponent<T>();
            if (comp != null) return comp;
            start = start.parent;
        }

        return default;
    }

    public static List<Transform> GetChildren(this Transform t)
    {
        var r = new List<Transform>();
        for (var i = 0; i < t.childCount; i++) r.Add(t.GetChild(i));
        return r;
    }

    public static void ForeachChildrenRecursively(this Transform t, Action<Transform> action)
    {
        if (t == null) return;
        action(t);
        for (var i = 0; i < t.childCount; i++)
            t.GetChild(i).ForeachChildrenRecursively(action);
    }

    public static void ForeachParentRecursively(this Transform t, Action<Transform> action)
    {
        if (t == null || t.parent == null) return;
        action(t.parent);
        t.parent.ForeachParentRecursively(action);
    }

    public static void ForeachChild(this Transform t, Action<Transform> a)
    {
        var children = new List<Transform>();
        for (var i = 0; i < t.childCount; i++)
        {
            var c = t.GetChild(i);
            children.Add(c);
        }

        foreach (var c in children)
        {
            if (c) a(c);
        }
    }

    #endregion

    public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

    #region UI
    
    public static void Show(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public static void Hide(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    #endregion
    #endregion
}