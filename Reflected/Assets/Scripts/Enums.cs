
/// <summary>
/// Horizontal and Vertical
/// </summary>
public enum Orientation
{
    Horizontal,
    Vertical
}

/// <summary>
/// North, East, South, West
/// </summary>
public enum CardinalDirection
{
    North,
    East,
    South,
    West
}

/// <summary>
/// North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
/// </summary>
public enum OrdinalDirection
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

/// <summary>
/// Forward, Backward, Left, Right, Up, Down
/// </summary>
public enum RelativeDirection
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

/// <summary>
/// True, Mirror
/// </summary>
public enum Dimension
{
    True,
    Mirror
}

/// <summary>
/// Start, Peaceful, Shop, Monster, Boss
/// </summary>
public enum RoomType
{
    Start,
    Peaceful,
    Shop,
    Monster,
    Boss
}

/// <summary>
/// Starts that effect the enemy
/// </summary>
public enum Stats
{
    Health,
    DamageReduction,
    MovementSpeed,
    Damage,
    AttackSpeed,
    AoE   
}

/// <summary>
/// Minimap layers
/// </summary>
public enum MinimapLayer
{
    Ground,
    Decorations,
    Walls,
    Objects,
    Player
}