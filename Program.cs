using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using AR_Lib.Geometry;
using AR_Lib.HalfEdgeMesh;
using AR_Lib.IO;
using AR_Lib.Curve;
using AR_Lib;

namespace AR_TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];

            TestHalfEdgeMesh(path);

            TestMatrix();

        }

        public static void TestMatrix()
        {
            Console.WriteLine("---- TestMatrix() called ----");

            Matrix<Point3d> matrix = new Matrix<Point3d>(2);
            matrix[0, 0] = new Point3d(5, 6, 8);
            matrix[0, 1] = new Point3d(3333, 6, 8);
            matrix[1, 1] = new Point3d(5, 5.22, 8);
            matrix[1, 0] = new Point3d(5, 6, 4.218);

            Console.WriteLine("Matrix[0,0] => " + matrix[0, 0]);
            Console.WriteLine("Matrix[0,1] => " + matrix[0, 1]);
            Console.WriteLine("Matrix[1,0] => " + matrix[1, 0]);
            Console.WriteLine("Matrix[1,1] => " + matrix[1, 1]);

            Console.WriteLine("---- TestMatrix() ended ----");
        }
        public static void TestHalfEdgeMesh(string path)
        {
            Console.WriteLine("---- TestHalfEdgeMesh() called ----");

            OFFMeshData data;
            OFFResult result = OFFReader.ReadMeshFromFile(path, out data);
            Debug.WriteLine("OFFReader result: " + result + "\n");


            HE_Mesh mesh = new HE_Mesh(data.vertices, data.faces);
            Debug.WriteLine(mesh);

            HE_MeshTopology top = new HE_MeshTopology(mesh);
            top.computeVertexAdjacency();
            top.computeFaceAdjacency();
            top.computeEdgeAdjacency();
            //Debug.WriteLine(top.TopologyDictToString(top.FaceVertex));

            Debug.WriteLine("isMesh? triangular: " + mesh.isTriangularMesh() + " quad: " + mesh.isQuadMesh() + " ngon: " + mesh.isNgonMesh());

            OFFResult result2 = OFFWritter.WriteMeshToFile(mesh, "/Users/alan/Desktop/AR_GeometryLibrary/AR_TerminalApp/meshes/cubeOut.off");
            Debug.WriteLine(result.ToString());

            mesh.Faces[0].HalfEdge.Vertex.UserValues.Add("set1", 3);
            mesh.Faces[0].HalfEdge.Next.Vertex.UserValues.Add("set1", 4);
            mesh.Faces[0].HalfEdge.Next.Next.Vertex.UserValues.Add("set1", 3);


            Line levelLine;
            AR_Lib.Curve.LevelSets.getFaceLevel("set1", 3.5, mesh.Faces[0], out levelLine);

            Console.WriteLine("---- TestHalfEdgeMesh() ended ----");

        }
    }
}
