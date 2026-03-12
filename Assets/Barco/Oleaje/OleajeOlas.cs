using UnityEngine;
using System.Collections.Generic;

public class GlobalSeaWave : MonoBehaviour
{
    [Header("Ajustes de Onda")]
    public float amplitude = 0.2f;
    public float frequency = 2.0f;
    public float speed = 1.5f;

    // Estructura para guardar los datos de cada hijo
    private class WaveChild
    {
        public Mesh mesh;
        public Vector3[] baseVertices;
        public Vector3[] workingVertices;
        public Transform transform;
    }

    private List<WaveChild> waveChildren = new List<WaveChild>();
    private bool initialized = false;

    void Start()
    {
        // Buscamos en todos los hijos que tengan un SpriteRenderer
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            if (sr.sprite == null) continue;

            // 1. Extraer datos y crear material con textura
            Vector2[] sVertices = sr.sprite.vertices;
            ushort[] sTriangles = sr.sprite.triangles;
            Vector2[] sUV = sr.sprite.uv;

            Material sMaterial = new Material(sr.material);
            sMaterial.mainTexture = sr.sprite.texture;

            // 2. Crear objeto visual para este hijo
            GameObject meshChild = new GameObject("Mesh_Visual_" + sr.name);
            meshChild.transform.SetParent(sr.transform);
            meshChild.transform.localPosition = Vector3.zero;
            meshChild.transform.localRotation = Quaternion.identity;

            // 3. Crear Mesh
            Mesh newMesh = new Mesh();
            newMesh.MarkDynamic();

            Vector3[] bVertices = new Vector3[sVertices.Length];
            for (int i = 0; i < sVertices.Length; i++)
                bVertices[i] = new Vector3(sVertices[i].x, sVertices[i].y, 0);

            newMesh.vertices = bVertices;
            newMesh.uv = sUV;

            int[] triangles = new int[sTriangles.Length];
            for (int i = 0; i < sTriangles.Length; i++)
                triangles[i] = (int)sTriangles[i];
            newMesh.triangles = triangles;

            // 4. Configurar Renderizado
            meshChild.AddComponent<MeshFilter>().mesh = newMesh;
            MeshRenderer mr = meshChild.AddComponent<MeshRenderer>();
            mr.material = sMaterial;
            mr.sortingLayerID = sr.sortingLayerID;
            mr.sortingOrder = sr.sortingOrder;

            // 5. Guardar en la lista para animar
            waveChildren.Add(new WaveChild
            {
                mesh = newMesh,
                baseVertices = bVertices,
                workingVertices = new Vector3[bVertices.Length],
                transform = sr.transform
            });

            sr.enabled = false; // Ocultar sprite original
        }
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        float time = Time.time * speed;

        foreach (var child in waveChildren)
        {
            for (int i = 0; i < child.baseVertices.Length; i++)
            {
                Vector3 v = child.baseVertices[i];
                // Usamos la posición X global de cada hijo para que la onda sea continua
                float worldX = v.x + child.transform.position.x;
                float wave = Mathf.Sin(worldX * frequency + time) * amplitude;

                if (v.y > -0.8f)
                {
                    v.y = child.baseVertices[i].y + wave;
                }
                child.workingVertices[i] = v;
            }
            child.mesh.vertices = child.workingVertices;
        }
    }
}