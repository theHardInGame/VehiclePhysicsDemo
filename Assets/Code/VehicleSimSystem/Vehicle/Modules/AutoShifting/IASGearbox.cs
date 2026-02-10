using System;

internal interface IASGearbox
{
    event Action<ShiftRequest> RequestShift;
    void Upshift();
    void Downshift();
}

internal enum ShiftRequest
{
    None,
    Upshift,
    Downshift
}