public static class Globals
{
    // layers
    public const int CharacterLayer = 6;
    public const int CharacterLayerMask = 1 << 6;
    public const int GroundLayerMask = 1 << 7;
    public const int WallLayerMask = 1 << 8;

    // tags
    public const string GroundTag = "Ground";

    // deltas
    public const float MoveInputDelta = 0.1f;
    public const float VelocityDelta = 0.01f;
    public const float DistanceDelta = 0.01f;
    public const float RaycastDelta = 1f;

    // distances
    public const float RaycastDistance = 3f;
    public const float GroundCheckDistance = 0.05f;

}
