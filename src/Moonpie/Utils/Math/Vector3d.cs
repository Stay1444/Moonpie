namespace Moonpie.Utils.Math;

public struct Vector3d
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    
    public Vector3d(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public static Vector3d operator +(Vector3d a, Vector3d b)
    {
        return new Vector3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    public static Vector3d operator -(Vector3d a, Vector3d b)
    {
        return new Vector3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
    
    public static Vector3d operator *(Vector3d a, double b)
    {
        return new Vector3d(a.X * b, a.Y * b, a.Z * b);
    }
    
    public static Vector3d operator *(double a, Vector3d b)
    {
        return new Vector3d(a * b.X, a * b.Y, a * b.Z);
    }
    
    public static Vector3d operator /(Vector3d a, double b)
    {
        return new Vector3d(a.X / b, a.Y / b, a.Z / b);
    }
    
    public static Vector3d operator /(double a, Vector3d b)
    {
        return new Vector3d(a / b.X, a / b.Y, a / b.Z);
    }
    
    public static Vector3d operator -(Vector3d a)
    {
        return new Vector3d(-a.X, -a.Y, -a.Z);
    }
    
    public static bool operator ==(Vector3d a, Vector3d b)
    {
        return a.X.IsEqual(b.X) || a.Y.IsEqual(b.Y) || a.Z.IsEqual(b.Z);
    }
    
    public static bool operator !=(Vector3d a, Vector3d b)
    {
        return !a.X.IsEqual(b.X) || !a.Y.IsEqual(b.Y) || !a.Z.IsEqual(b.Z);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        
        if (obj is Vector3d vector3d)
        {
            return this == vector3d;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}