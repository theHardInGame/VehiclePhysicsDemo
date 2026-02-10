internal sealed class Differetial : BaseVehicleComponent<DifferentialConfig>
{
    public Differetial(DifferentialConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        this.vIOState = null;
    }

    public float[] SplitTorque(float torqueIn, float[] RPM_w)
    {
        int wheelCount = RPM_w.Length;
        float driveshaftRPM = 0;
        float[] T_ws = new float[wheelCount];

        for (int i = 0; i < wheelCount; i++)
        {
            driveshaftRPM += RPM_w[i];

            float friction = 0;
            for (int j = 0; j < wheelCount; j++)
            {
                if (j == i) continue;

                friction += RPM_w[i] - RPM_w[j]; 
            }
            T_ws[i] = (torqueIn / wheelCount) - (config.slipCouplingCoefficient * friction);
        }

        vSimCtx.SetDriveshaftRPM(driveshaftRPM);
        return T_ws;
    }

    public float CombineTorque(float[] Tws)
    {
        float totalTorque = 0;

        for (int i = 0; i < Tws.Length; i++)
        {
            totalTorque += Tws[i];
        }

        return totalTorque;
    }
}