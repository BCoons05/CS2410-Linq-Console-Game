using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
    /// <summary>
    /// Class for a machine gun IWeapon. Holds the bullets in magazine, mag size, damage, reload speed,
    /// type and rarity
    /// </summary>
    class MachineGun : IWeapon
    {
        public WeaponTypes Type { get; }
        public Rarities Rarity { get; }
        public int Damage { get; }
        public int FireRate { get; }
        public int MagazineSize { get; }
        public double ReloadSpeed { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        private Player player;
        private Background background;

        public MachineGun(WeaponTypes type, Rarities rarity, int magSize, int damage, double reloadSpeed, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = damage;
            MagazineSize = magSize;
            ReloadSpeed = reloadSpeed;
            FireRate = rarity == Rarities.COMMON ? 2 : 1;
            this.player = player;
            this.background = background;
            BulletsInMagazine = MagazineSize;
        }

        /// <summary>
        /// Creates a projectile after space is pressed. The projectile is saved to a list and returned to be drawn
        /// </summary>
        /// <returns>List<Projectile> that holds all projectiles to be fired</returns>
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();
            BulletsInMagazine--;
            int position = player.Position;

            if (player.Position == background.LeftWall + 1)
                position = player.Position + 1;
            else if (player.Position == background.RightWall - 1)
                position = player.Position - 1;

            Projectile projectile = new Projectile(ProjectileType.BULLET, position, player.Bottom - 1, player.Direction, FireRate, Damage, background);
            shots.Add(projectile);
            return shots;
        }

        /// <summary>
        /// Sets bulletsinmagazine to be equal to magazine size
        /// </summary>
        public void Reload()
        {
            BulletsInMagazine = MagazineSize;
        }

        /// <summary>
        /// Sets the weapon as equipped
        /// </summary>
        public void Equip()
        {
            Equipped = true;
        }

        public override string ToString()
        {
            return $"{Rarity} {Type} Dam:{Damage} Mag:{MagazineSize} Reload:{ReloadSpeed}";
        }
    }
}
