public struct CollisionInfo
{
    public bool above, below, left, right; 
    public void Reset()
    {
        above = below = left = right = false;
    }
}
