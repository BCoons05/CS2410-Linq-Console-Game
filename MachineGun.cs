using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
    class MachineGun : IWeapon
    {
        public WeaponTypes Type { get; }
        public Rarities Rarity { get; }
        public int Damage { get; }
        public int FireRate { get; }
        public int MagazineSize { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        private Player player;
        private Background background;

        public MachineGun(WeaponTypes type, Rarities rarity, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = rarity == Rarities.COMMON ? 5 : rarity == Rarities.RARE ? 8 : 10 ;
            FireRate = rarity == Rarities.COMMON ? 2 : 1;
            this.player = player;
            this.background = background;
            MagazineSize = 20;
            BulletsInMagazine = MagazineSize;
        }
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();
            BulletsInMagazine--;
            Projectile projectile = new Projectile(ProjectileType.BULLET, player.Position, player.Bottom - 1, player.Direction, FireRate, Damage, background);
            shots.Add(projectile);
            return shots;
        }

        public void Reload()
        {
            BulletsInMagazine = MagazineSize;
        }

        public void Equip()
        {
            Equipped = true;
        }

        public override string ToString()
        {
            return $"{Rarity} {Type} Damage: {Damage} Mag size: {MagazineSize}";
        }
    }
}
