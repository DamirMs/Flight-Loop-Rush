
[System.Serializable]
public class ArraySerializationForEditor<T>
{
    public T[] Objects;

    public ArraySerializationForEditor(T[] objects)
    {
        Objects = objects;
    }

    public static ArraySerializationForEditor<T>[] From2DArray(T[,] array2D)
    {
        int m = array2D.GetLength(0), n = array2D.GetLength(1);

        var result = new ArraySerializationForEditor<T>[m];

        for (int i = 0; i < m; i++)
        {
            T[] row = new T[n];

            for (int j = 0; j < n; j++) row[j] = array2D[i, j];

            result[i] = new ArraySerializationForEditor<T>(row);
        }

        return result;
    }

    public static T[,] To2DArray(ArraySerializationForEditor<T>[] serializedArray)
    {
        int m = serializedArray.Length, n = serializedArray[0].Objects.Length;

        T[,] result = new T[m, n];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = serializedArray[i].Objects[j];
            }
        }

        return result;
    }
}