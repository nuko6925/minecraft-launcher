using CmlLib.Core;

namespace Minecraft_Launcher;

public class CustomPath : MinecraftPath
{
    public CustomPath(string p)
    {
        BasePath = NormalizePath(p);

        Library = NormalizePath(BasePath + "/libs");
        Versions = NormalizePath(BasePath + "/vers");
        Resource = NormalizePath(BasePath + "/resources");

        Runtime = NormalizePath(BasePath + "/java");
        Assets = NormalizePath(BasePath + "/assets");

        CreateDirs();
    }

    public CustomPath(MinecraftPath p)
    {
        BasePath = NormalizePath(p.BasePath);
        Library = NormalizePath(p.Library);
        Versions = NormalizePath(p.Versions);
        Resource = NormalizePath(p.Resource);
        Runtime = NormalizePath(p.Runtime);
        Assets = NormalizePath(p.Assets);
        CreateDirs();
    }

    public override string GetVersionJarPath(string id)
        => NormalizePath($"{Versions}/{id}/client.jar");
    
    public override string GetVersionJsonPath(string id)
        => NormalizePath($"{Versions}/{id}/client.json");

    public override string GetAssetObjectPath(string assetId)
        => NormalizePath($"{Assets}/files");
    
}