public static class Globals
{
    // layers
    public const int PlayerLayer = 6;
    public const int PlayerLayerMask = 1 << 6;
    public const int GroundLayerMask = 1 << 7;
    public const int WallLayerMask = 1 << 8;
    public const int EnemyLayerMask = 1 << 9;
    public const int EntityLayerMask = EnemyLayerMask | PlayerLayerMask;

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

    // limits
    public const int MaxEntitiesHitAtOnce = 10;


}
