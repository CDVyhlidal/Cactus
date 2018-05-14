namespace Cactus.Interfaces
{
    public interface IVersionManager
    {
        bool Is100(string version);
        bool Is107(string version);
        bool Is107Beta(string version);
        bool Is114OrNewer(string version);
        bool IsPreLod(string version);
        bool RequiresPatchFile(string version);
    }
}
