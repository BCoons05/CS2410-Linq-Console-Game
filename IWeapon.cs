using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
    enum WeaponTypes { SHOTGUN, MACHINEGUN, ROCKETLAUNCHER }
    enum Rarities { COMMON, RARE, LEGENDARY }
    interface IWeapon
    {
        WeaponTypes Type { get; }
        Rarities Rarity { get; }
        int Damage { get; }
        int FireRate { get; }
        int MagazineSize { get; }
        int BulletsInMagazine { get; }
        bool Equipped { get; }
        List<Projectile> Fire();
        void Reload();
        void Equip();
        string ToString();
    }
}
