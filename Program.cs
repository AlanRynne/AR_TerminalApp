using System;
using System.Diagnostics;
using System.Collections.Generic;
using AR_Lib.Geometry;
using AR_Lib.HalfEdgeMesh;
using AR_Lib.IO;

namespace AR_TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];
            OFFMeshData data;
            OFFResult result = OFFReader.ReadMeshFromFile(path, out data);
            Debug.WriteLine("OFFReader result: " + result + "\n");


            HE_Mesh mesh = new HE_Mesh(data.vertices, data.faces);
            Debug.Write(mesh);

            foreach (HE_Vertex v in mesh.Vertices)
            {
                List<HE_Vertex> vertices = v.adjacentVertices();
                List<HE_Edge> edges = v.adjacentEdges();
                List<HE_Face> faces = v.adjacentFaces();

                Debug.WriteLine("ADJACENT: V " + vertices.Count + " F " + faces.Count + " E " + edges.Count);
            }
            
            HE_MeshTopology top = new HE_MeshTopology(mesh);
            top.computeVertexAdjacency();
        }
    }
}
