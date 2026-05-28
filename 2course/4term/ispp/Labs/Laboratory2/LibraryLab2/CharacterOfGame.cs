using System.Reflection.Emit;
using System.Text.Json;

namespace LibraryLab2
{
    public class CharacterOfGame
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool HaveWeapon { get; set; }

        public CharacterOfGame(long id, string name, bool haveWeapon)
        {
            Id = id;
            Name = name;
            HaveWeapon = haveWeapon;
        }

        public override string ToString()
        {
            return $"{Id},{Name},{HaveWeapon}"; 
        }
    }
}
