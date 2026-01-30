internal interface IDrivetrainComponent
{
    ForwardState Forward(ForwardState input, float tick);
    BackwardState Backward(BackwardState input, float tick);
}