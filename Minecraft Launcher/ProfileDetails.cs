namespace Minecraft_Launcher;

public struct ProfileDetails : IEquatable<ProfileDetails>
{
    public string Name;
    public string Version;
    public string Type;
    public string Dir;
    public string Jvm;
    public string Runtime;

    public bool Equals(ProfileDetails other)
    {
        return Name == other.Name && Version == other.Version && Type == other.Type && Dir == other.Dir && Jvm == other.Jvm && Runtime == other.Runtime;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProfileDetails other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Version, Type, Dir, Jvm, Runtime);
    }

    public static bool operator ==(ProfileDetails left, ProfileDetails right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ProfileDetails left, ProfileDetails right)
    {
        return !(left == right);
    }
}