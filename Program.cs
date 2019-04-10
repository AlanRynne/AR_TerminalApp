using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

using AR_Lib;

using AR_Lib.Geometry;
using AR_Lib.HalfEdgeMesh;
using AR_Lib.IO;
using AR_Lib.Curve;
using AR_Lib.Geometry;
using AR_Lib.Collections;
using System.IO;

namespace AR_TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];

            TestHalfEdgeMesh(path);

            TestMatrix();

            TestNurbsSurface();

            Debug.WriteLine("---- FINISH -----");

        }

        public static void TestNurbsSurface()
        {
            Stopwatch stp = new Stopwatch();

            Random rnd = new Random();

            Debug.WriteLine("----- TestNurbsSurface Test Started -----");
            stp.Start();
            int gridsize = 20;
            List<Point4d> pts = new List<Point4d>(16);
            List<double> tmp = new List<double>();

            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    pts.Add(new Point4d(i * 1000, j * 1000, Math.Sqrt(i * j) * 10, 1));
                    tmp.Add((double)j);

                }
            }

            // List<double> uKnts = new List<double> { 0, 0, 0, 0, 0.33, 0.33, 0.33, 0.33, 0.66, 0.66, 0.66, 0.66, 1, 1, 1, 1 };
            // List<double> vKnts = new List<double> { 0, 0, 0, 0, 0.33, 0.33, 0.33, 0.33, 0.66, 0.66, 0.66, 0.66, 1, 1, 1, 1 };
            List<double> uKnts = tmp;
            List<double> vKnts = tmp;

            Surface a = new Surface(1, 1, gridsize, gridsize, pts, uKnts, vKnts, 40, 40);
            a.TessellateSurface();
            Debug.WriteLine(a);
            stp.Stop();

            Debug.WriteLine("Execution time: " + stp.Elapsed);

            File.WriteAllText("surfPoints.txt", "");

            using (StreamWriter sw = new StreamWriter("/Users/alan/Desktop/AR_GeometryLibrary/AR_TerminalApp/surfPoints.txt"))
            {
                foreach (Point3d pt in a.Vertices)
                {
                    sw.WriteLine(pt.X + "; " + pt.Y + "; " + pt.Z);
                }
            }


        }

        public static void TestReadSettings()
        {
            Settings set = SettingsReader.ReadSettings();
            Debug.WriteLine("----- ReadSettings Test Started -----");
            Debug.WriteLine(set.Constants.Tolerance);
        }

        public static void TestMatrix()
        {
            Debug.WriteLine("---- TestMatrix() called ----");

            Matrix<Point3d> matrix = new Matrix<Point3d>(2);
            matrix[0, 0] = new Point3d(5, 6, 8);
            matrix[0, 1] = new Point3d(3333, 6, 8);
            matrix[1, 1] = new Point3d(5, 5.22, 8);
            matrix[1, 0] = new Point3d(5, 6, 4.218);

            Debug.WriteLine("Matrix[0,0] => " + matrix[0, 0]);
            Debug.WriteLine("Matrix[0,1] => " + matrix[0, 1]);
            Debug.WriteLine("Matrix[1,0] => " + matrix[1, 0]);
            Debug.WriteLine("Matrix[1,1] => " + matrix[1, 1]);

            Debug.WriteLine("---- TestMatrix() ended ----");
        }
        public static void TestHalfEdgeMesh(string path)
        {
            Debug.WriteLine("---- TestHalfEdgeMesh() called ----");

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

            Debug.WriteLine("---- TestHalfEdgeMesh() ended ----");

        }
    }
}
