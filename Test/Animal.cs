

namespace Zoo
{
    public class Animal : INameable
    {
        private string _name;
        public string Name { get; set; }


        public Animal(string name)
        {
            Name = name;
        }
    }

public class Human : INameable
    {
        public string Name { get; set; }

        public Human(string name)
        {
            Name = name;
        }
    }



}
