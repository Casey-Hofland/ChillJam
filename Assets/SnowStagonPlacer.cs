using UnityEngine;

public class SnowStagonPlacer : MonoBehaviour
{
    public GameObject snowstagon;
    [Min(0)] public int layers = 1;

    [ContextMenu("Delete All Children")]
    private void DeleteAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    [ContextMenu("Sin")]
    public void Sin()
    {
        for (float i = 0f; i < 2.1f; i += 0.1f)
        {
            Debug.Log($"{i}: {Mathf.Sin(i * Mathf.PI)}");
        }
    }

    [ContextMenu("Place snowstagon")]
    private void PlaceSnowstagons()
    {
        DeleteAllChildren();

        UnityEditor.PrefabUtility.InstantiatePrefab(snowstagon, transform);

        var rot60y = Quaternion.Euler(0f, 60f, 0f);
        var direction = rot60y * transform.right * 1.732329f * snowstagon.transform.localScale.x;

        for (int i = 1; i <= layers; i++)
        {
            var position = transform.position + direction * i;

            direction = rot60y * direction;

            for (int j = 0; j < i * 6; j++)
            {
                if (j % i == 0)
                {
                    direction = rot60y * direction;
                }

                var prefab = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(snowstagon, transform);
                prefab.transform.position = position;

                position += direction;
            }

            direction = Quaternion.Inverse(rot60y) * direction;
        }

        //for (int i = 1; i <= layers; i++)
        //{
        //    for (int j = 0; j < i * 6; j++)
        //    {
        //        //var position = Vector3.right * 17.32329f * i;

        //        Vector3 position;

        //        if (j % i == 0)
        //        {
        //            position = Vector3.right * 17.32329f * i;
        //        }
        //        else
        //        {
        //            position = Vector3.right * 15f * i;
        //        }

        //        position = Vector3.right * (17.32329f - Mathf.Sin((j % i) / (float)i * Mathf.PI) * 2.32329f) * i;

        //        //if (i == 2)
        //        //{
        //        //    if (j % 2 == 0)
        //        //    {
        //        //        position = Vector3.right * 17.32329f * i;
        //        //    }
        //        //    else
        //        //    {
        //        //        position = Vector3.right * 15f * i;
        //        //    }
        //        //}

        //        var posRotation = Quaternion.Euler(0f, 60f / i * j, 0f);

        //        position = posRotation * position;

        //        //if (i == 2)
        //        //{
        //        //    if (j % 2 == 0)
        //        //    {
        //        //        position /= (30f / 35f);
        //        //    }
        //        //    else
        //        //    {
        //        //        position *= (30f / 35f);
        //        //    }
        //        //}

        //        var prefab = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(snowstagon, transform);
        //        prefab.transform.position = position;
        //    }
        //}
    }
}
