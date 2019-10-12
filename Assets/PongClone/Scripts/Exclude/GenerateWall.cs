using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    [RequireComponent(typeof(MeshFilter))]
    public class GenerateWall : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        public Rect rect;

        void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = new Mesh();
        }

        void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                List<Vector3> vertices = new List<Vector3>();
                vertices.Add(new Vector3(rect.x, rect.y, 0));
                vertices.Add(new Vector3(rect.x + Random.Range(0.5f, 1.0f), rect.y + Random.Range(0.5f, 1.5f), 0));
                vertices.Add(new Vector3(rect.x, vertices[vertices.Count - 1].y, 0));
                Color[] colorElems = new Color[] { new Color(0.125f, 0.125f, 0.125f), new Color(0.5f, 0.5f, 0.5f), new Color(1f, 1f, 1f) };
                List<Color> colors = new List<Color>();
                List<int> indices = new List<int>() { 0, 2, 1 };
                int i = indices.Count - 1;
                while (true)
                {
                    float y = vertices[vertices.Count - 1].y + Random.Range(0.5f, 1.5f);
                    if (y >= rect.height)
                    {
                        break;
                    }
                    vertices.Add(new Vector3(vertices[vertices.Count - 1].x + Random.Range(0.5f, 1.0f), y, 0));
                    vertices.Add(new Vector3(rect.x, vertices[vertices.Count - 1].y, 0));
                    indices.Add(i);
                    indices.Add(i + 1);
                    indices.Add(i - 1);
                    indices.Add(i);
                    indices.Add(i + 2);
                    indices.Add(i + 1);
                    i += 2;
                }
                vertices.Add(new Vector3(rect.x, rect.height, 0));
                indices.Add(i);
                indices.Add(i + 1);
                indices.Add(i - 1);
                _meshFilter.mesh.SetVertices(vertices);
                _meshFilter.mesh.SetTriangles(indices, 0);
                foreach (Vector3 v in vertices)
                {
                    colors.Add(colorElems[Random.Range(0, colorElems.Length)]);
                }
                _meshFilter.mesh.SetColors(colors);
                gameObject.AddComponent<PolygonCollider2D>();
            }
        }
    }
}