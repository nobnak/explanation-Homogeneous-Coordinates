using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestSolver : MonoBehaviour {

    private void OnEnable() {
        var V = Vector<double>.Build;
        var M = Matrix<double>.Build;

        var pp = new Vector<double>[] {
            V.Dense(new double[]{ 0, 0 }),
            V.Dense(new double[]{ 0, 1 }),
            V.Dense(new double[]{ 1, 1 }),
            V.Dense(new double[]{ 1, 0 }),
        };
        var qq = new Vector<double>[] {
            V.Dense(new double[]{ 0, 0 }),
            V.Dense(new double[]{ 0, 1 }),
            V.Dense(new double[]{ 1, 1 }),
            V.Dense(new double[]{ 1, 0 }),
        };

        var bufA = new List<double[]>();
        var bufB = new List<double>();
        for(var i = 0; i < pp.Length; i++) {
            var p = pp[i];
            var q = qq[i];
            bufA.Add(new double[] { 0, 0, 0, p[0], p[1], 1, -q[1] * p[0], -q[1] * p[1] });
            bufA.Add(new double[] { p[0], p[1], 1, 0, 0, 0, -q[0] * p[0], -q[0] * p[1] });
            bufB.Add(q[1]);
            bufB.Add(q[0]);
        }
        var A = M.DenseOfRowArrays(bufA);
        var b = V.DenseOfEnumerable(bufB);
        var vecH = V.Dense(9, 1);
        vecH.SetSubVector(0, 8, A.Solve(b));

        var H = M.DenseOfRowMajor(3, 3, vecH);
        //Debug.Log(A);
        //Debug.Log(b);
        //Debug.Log(H);
    }
}
