internal interface IDrivetrainModule
{
    internal ForwardState Forward(ForwardState input);
    internal BackwardState Backward(BackwardState input);
}