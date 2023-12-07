namespace Products.Models.Entities;

[Flags]
public enum Region : ulong
{
    None = 0,
    BrNorthEast = 1 << 0,
    BrSouthEast = 1 << 1,
    BrSouth = 1 << 2,
    BrCenterWest = 1 << 3,
    BrNorth = 1 << 4,
    BrAll = BrNorthEast | BrSouthEast | BrSouth | BrCenterWest | BrNorth

}
