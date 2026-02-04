using System;
using System.Numerics;

internal sealed class VehicleBody : BaseVehicleComponent<VehicleBodyConfig>
{
    public VehicleBody(VehicleBodyConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        totalWeight = vIOState.vSimCtx.GetMass() * vIOState.vSimCtx.GetGravity();
        localCG = vIOState.vSimCtx.GetCGLocal();

        B = new float[,]
        {
            { totalWeight },
            { 0f },
            { 0f }
        };
    }

    private Vector3 localCG;
    private float totalWeight;
    private readonly float[,] B;

    public VehicleBodyOutput DistributeLoad(VehicleBodyInput input)
    {
        float[,] A = new float[3, input.suspensionCount];
        float[] K = new float[input.suspensionCount];

        for (int i = 0; i < input.suspensionLocalPositions.Length; i++)
        {
            Vector3 r = input.suspensionLocalPositions[i] - localCG;
            float M_x = r.Z * totalWeight;
            float M_z = r.X * totalWeight;

            A[0,i] = 1f;
            A[1,i] = M_z;
            A[2,i] = M_x;

            K[i] = input.isGrounded[i] ? input.springRates[i] : 0f;
        }

        float[,] AK = MultiplyDiagonal(A, K);

        float[,] AKAT = Multiply(AK, Transpose(A));

        float[,] AKATInv = Invert3x3(AKAT); // 3x3

        // 4. Compute A^T * AKATInv * B
        float[,] AT = Transpose(A); // n x 3
        float[,] AT_AKATInv = Multiply(AT, AKATInv); // n x 3
        float[,] AT_AKATInv_B = Multiply(AT_AKATInv, B); // n x 1

        // 5. Multiply by K (diagonal) to get F
        float[] F = MultiplyDiagonalVector(K, Flatten(AT_AKATInv_B));

        VehicleBodyOutput output = new();
        output.loadPerSuspension = F;

        return output;
    }

    private static float[,] Multiply(float[,] A, float[,] B)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1);
        int p = B.GetLength(1);
        float[,] C = new float[m, p];

        for (int i = 0; i < m; i++)
            for (int j = 0; j < p; j++)
                for (int k = 0; k < n; k++)
                    C[i, j] += A[i, k] * B[k, j];

        return C;
    }

    private static float[,] MultiplyDiagonal(float[,] A, float[] K)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1);
        float[,] C = new float[m, n];

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                C[i, j] = A[i, j] * K[j]; // K[j] on diagonal

        return C;
    }

    private static float[] MultiplyDiagonalVector(float[] K, float[] F)
    {
        int n = K.Length;
        float[] R = new float[n];
        for (int i = 0; i < n; i++) R[i] = K[i] * F[i];
        return R;
    }

    private static float[,] Invert3x3(float[,] m)
    {
        float a = m[0,0], b = m[0,1], c = m[0,2];
        float d = m[1,0], e = m[1,1], f = m[1,2];
        float g = m[2,0], h = m[2,1], i = m[2,2];

        float det = a*(e*i - f*h) - b*(d*i - f*g) + c*(d*h - e*g);
        if (Math.Abs(det) < 1e-6f) throw new Exception("Matrix not invertible");

        float invDet = 1f / det;
        float[,] inv = new float[3,3];

        inv[0,0] =  (e*i - f*h) * invDet;
        inv[0,1] = -(b*i - c*h) * invDet;
        inv[0,2] =  (b*f - c*e) * invDet;
        inv[1,0] = -(d*i - f*g) * invDet;
        inv[1,1] =  (a*i - c*g) * invDet;
        inv[1,2] = -(a*f - c*d) * invDet;
        inv[2,0] =  (d*h - e*g) * invDet;
        inv[2,1] = -(a*h - b*g) * invDet;
        inv[2,2] =  (a*e - b*d) * invDet;

        return inv;
    }

    private static float[,] Transpose(float[,] A)
    {
        int m = A.GetLength(0), n = A.GetLength(1);
        float[,] T = new float[n,m];
        for (int i=0;i<m;i++)
            for (int j=0;j<n;j++)
                T[j,i] = A[i,j];
        return T;
    }

    private static float[] Flatten(float[,] A)
    {
        int m = A.GetLength(0), n = A.GetLength(1);
        float[] v = new float[m*n];
        for (int i=0;i<m;i++)
            for (int j=0;j<n;j++)
                v[i*n+j] = A[i,j];
        return v;
    }
}

internal struct VehicleBodyInput
{
    public Vector3[] suspensionLocalPositions;
    public float[] springRates;
    public bool[] isGrounded;
    public int suspensionCount;
}

internal struct VehicleBodyOutput
{
    public float[] loadPerSuspension;
}