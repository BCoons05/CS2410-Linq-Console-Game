using System;
using System.Collections.Generic;

namespace ConsolePlatformer
{
    class RocketLauncher : IWeapon
    {
        public WeaponTypes Type { get; }
        public Rarities Rarity { get; }
        private ConsoleColor ProjectileColor;
        public int Damage { get; }
        public int FireRate { get; }
        public int MagazineSize { get; }
        public double ReloadSpeed { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        private Player player;
        private Background background;

        public RocketLauncher(WeaponTypes type, Rarities rarity, int magSize, int damage, double reloadSpeed, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = damage;
            MagazineSize = magSize;
            ReloadSpeed = reloadSpeed;
            FireRate = rarity == Rarities.COMMON ? 5 : 3;
            this.player = player;
            this.background = background;
            BulletsInMagazine = MagazineSize;
            ProjectileColor = rarity == Rarities.LEGENDARY ? ConsoleColor.Red : rarity == Rarities.RARE ? ConsoleColor.Green : ConsoleColor.DarkYellow;
        }

        /// <summary>
        /// Creates a projectile after space is pressed. The projectile is saved to a list and returned to be drawn
        /// </summary>
        /// <returns>List<Projectile> that holds all projectiles to be fired</returns>
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();
            BulletsInMagazine--;
            Projectile projectile = new Projectile(ProjectileType.ROCKET, player.Position, player.Bottom - 1, player.Direction, 5, Damage, ProjectileColor, background);
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
