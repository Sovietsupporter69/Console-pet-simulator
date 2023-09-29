class Pet
{
    public string Name;
    public string LastName;
    public float Age;
    public float Hunger;
    public float Intelegence;
    public float Fitness;
    public float Happiness;
    public char Nature;
    public int Generation;
    private char NewNature;
    public int Spec;

    int RandNature;
    Random Rnd = new Random();

    public Pet()
    {
        this.Age = 0;
        this.Hunger = 0;
        this.Intelegence = 0;
        this.Fitness = 50;
        this.Happiness = 0;
        this.Generation = 1;
        this.Spec = 0;

        switch (Rnd.Next(1, 11)) {
            case 1:
                this.Nature = 'L'; break; //Lethargic: less fitness gain, less hapiness loss
            case 2:
                this.Nature = 'I'; break; //Intelegent: massivly reduced fitness gain, massivly increased Intelegence gain
            case 3:
                this.Nature = 'R'; break; //Rock brain: increased fitness gain, reduced intelegence gain
            case 4:
                this.Nature = 'J'; break; //Joyful: extra happiness gain and reduced happiness loss, reduced fitness and intelegence gain
            case 5:
                this.Nature = 'H'; break; //Hoarder: extra hunger storage and reduced hunger loss, reduced intelegence and happiness gain
            case 6:
                this.Nature = 'C'; break; //Clumsy: chance for any task to fail, chance for any task to get a boost in gains
            case 7:
                this.Nature = 'B'; break; //Blessed: increased critical chances, slightly reduced intelegence, happiness and fitness gain
            case 8:
                this.Nature = 'P'; break; //Playboy: changes some comments said
            case 9:
                this.Nature = 'S';        //Specialised: choose a stat to get massive increases in, all others have reduced gain (including hunger)
                Special();  break;
            case 10:
                this.Nature = 'K'; break; //Chaotic: chance to change a portion of the stat increased to another random stat.
        }
    }
    public Pet(string Name, float Age, float Hunger)
    {
        this.Name = Name;
        this.LastName = Name;
    }
    public char RandomiseNature()
    {
        switch (Rnd.Next(1, 11))
        {
            case 1:
                NewNature = 'L'; break;
            case 2:
                NewNature = 'I'; break;
            case 3:
                NewNature = 'R'; break;
            case 4:
                NewNature = 'J'; break;
            case 5:
                NewNature = 'H'; break;
            case 6:
                NewNature = 'C'; break;
            case 7:
                NewNature = 'B'; break;
            case 8:
                NewNature = 'P'; break;
            case 9:
                NewNature = 'S';
                Special();  break;
            case 10:
                NewNature = 'K'; break;
        }
        return NewNature;
    }
    private void Special()
    {
        Console.WriteLine("You have a specialised pet, please chose a specialisation: "); //Asks the user which specialisation they want
        Console.WriteLine("1. Hunger");
        Console.WriteLine("2. Happiness");
        Console.WriteLine("3. Intelegence");
        Console.WriteLine("4. Fitness");
        Console.Write("Your choice: ");
        while (true)
        { //Preventing going forward without an answer
            try
            {
                this.Spec = int.Parse(Console.ReadLine()); //Getting the users input
                if (this.Spec <= 4 && this.Spec >= 0) { break; }
            }
            catch { }
        }
    }
}
