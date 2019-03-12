using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using AR_Lib.Geometry;
using AR_Lib.HalfEdgeMesh;
using AR_Lib.IO;
using AR_Lib.Curve;

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

            HE_MeshTopology top = new HE_MeshTopology(mesh);
            top.computeVertexAdjacency();
            top.computeFaceAdjacency();
            top.computeEdgeAdjacency();
            //Debug.WriteLine(top.TopologyDictToString(top.FaceVertex));

            Debug.WriteLine("isMesh? triangular: " + mesh.isTriangularMesh() + " quad: " + mesh.isQuadMesh() + " ngon: " + mesh.isNgonMesh());

            OFFResult result2 = OFFWritter.WriteMeshToFile(mesh, "/Users/alan/Desktop/AR_GeometryLibrary/AR_TerminalApp/meshes/cubeOut.off");
            Debug.WriteLine(result.ToString());

            mesh.Faces[0].HalfEdge.Vertex.UserValues.Add("set1",3);
            mesh.Faces[0].HalfEdge.Next.Vertex.UserValues.Add("set1",4);
            mesh.Faces[0].HalfEdge.Next.Next.Vertex.UserValues.Add("set1",3);


            Line levelLine;
            
            AR_Lib.Curve.LevelSets.getFaceLevel("set1",3.5,mesh.Faces[0], out levelLine);

        }
    }
}
