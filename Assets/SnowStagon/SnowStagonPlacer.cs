using UnityEngine;

public class SnowStagonPlacer : MonoBehaviour
{
    public GameObject snowstagon;
    [Min(0)] public int layers = 1;

#if UNITY_EDITOR
    [ContextMenu("Delete All Children")]
    private void DeleteAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
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
    }
#endif
}
