internal interface IJunctionComponent<Tin, TOut>
{
    TOut Backward(TOut feedback, float dt);
    Tin Forward(Tin input, float dt);
}