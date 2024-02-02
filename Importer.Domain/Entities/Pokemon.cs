using Importer.Domain.Enums;

namespace Importer.Domain.Entities
{
    public class Pokemon
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EPokemonType Type { get; private set; }
        public int Total { get; private set; }
        public int HP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int SpeedAttack { get; private set; }
        public int SpeedDefense { get; private set; }
        public int Speed { get; private set; }

        public virtual ICollection<MyPokemon> MyPokemons { get; protected set; }

        public Pokemon(int id, string name, EPokemonType type, int total, int hP, int attack, int defense, int speedAttack, int speedDefense, int speed)
        {
            Id = id;
            Name = name;
            Type = type;
            Total = total;
            HP = hP;
            Attack = attack;
            Defense = defense;
            SpeedAttack = speedAttack;
            SpeedDefense = speedDefense;
            Speed = speed;
        }

        protected Pokemon()
        {
            
        }
    }
}
