using System.Runtime.ConstrainedExecution;
using System.Xml.Serialization;

Random rand = new Random();
bool Play = true;
Pet[] Pets = new Pet[10];
int PetCount = 0;
int CurrentPet = 0;
int Action = 99;
float CriticalMulti = 1;
int HungExtCapacity = 0;
bool FailChance; int ClumsyRand;
int CriticalRanMax = 10;  int CriticalRand;
bool PlayBoyActive; string CurrentNature;

int HungerFill; int HappyFill;
int IntelFill; int FitnsFill;
float HungMulti = 1; float HungRedMulti = 1;
float HapiMulti = 1; float HapiRedMulti = 1;
float InteMulti = 1; float InteRedMulti = 1;
float FitnMulti = 1; float FitnRedMulti = 1;

int NatureRand; float StatRand;
int Specialisation; float ChaosPercent;
bool Chaos = false; float ChaosChance = 0.7f;

/*List<Char> printableChars = new List<char>();
for (int i = char.MinValue; i <= char.MaxValue; i++) { //Extra bit of code for finding all possible characters
    char c = Convert.ToChar(i);
    if (!char.IsControl(c)) { printableChars.Add(c); }
}
Console.WriteLine(printableChars.ToArray());*/

CreatePet();
while (Play == true)
{
    HungerFill = 20; HappyFill = 20; //Reseting the bars so they can be reduced apropriatly
    IntelFill = 20; FitnsFill = 20;

    Console.Clear();
    if (CriticalMulti > 1) { Console.WriteLine("Your last turn was a Critical! you got an increased stat gain"); }
    Console.WriteLine(Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + ":");
    
    Console.Write("Hunger: ");                                    //all the stat bars are made using some simple maths, also displays nature
    HungerFill -= ((100+(20*HungExtCapacity)) - (int)(Pets[CurrentPet].Hunger))/5 + (int)(0.2f*HungExtCapacity);
    for (int hu1 = 0; hu1 < HungerFill; hu1++) { Console.Write("█"); }
    for (int hu2 = 0; hu2 < 20-HungerFill; hu2++) { Console.Write("-"); }
    Console.WriteLine("");
    Console.Write("Happiness: ");
    HappyFill -= (100 - (int)(Pets[CurrentPet].Happiness)) / 5;
    for (int ha1 = 0; ha1 < HappyFill; ha1++) { Console.Write("█"); }
    for (int ha2 = 0; ha2 < 20 - HappyFill; ha2++) { Console.Write("-"); }
    Console.WriteLine("");
    Console.Write("Intelegence: ");
    if (Pets[CurrentPet].Intelegence <= 200) {
        IntelFill -= (100 - (int)(Pets[CurrentPet].Intelegence)) / 5;
        for (int in1 = 0; in1 < IntelFill; in1++) { Console.Write("█"); }
    } else {
        Console.Write((int)Pets[CurrentPet].Intelegence);
    }
    Console.WriteLine("");
    Console.Write("Fitness: ");
    if (Pets[CurrentPet].Fitness <= 200) {
        FitnsFill -= (100 - (int)(Pets[CurrentPet].Fitness)) / 5;
        for (int fi1 = 0; fi1 < FitnsFill; fi1++) { Console.Write("█"); }
    } else {
        Console.Write((int)Pets[CurrentPet].Fitness);
    }
    Console.WriteLine("");
    Console.WriteLine("Nature: " + CurrentNature);
    Console.WriteLine("Age: " + (int)Pets[CurrentPet].Age + "  Gen: " + Pets[CurrentPet].Generation);

    Console.WriteLine("Chose what you want to do with " + Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + ":"); //Simply displaying the potencal actions a user can take
    Console.WriteLine("1.Feed");
    Console.WriteLine("2.Play");
    Console.WriteLine("3.Read");
    Console.WriteLine("4.Exersise");
    Console.WriteLine("5.Switch pet");
    Console.WriteLine("6.New pet");
    Console.Write("Your choice: ");
    while (true) { //Ensuring the user can only go forward with an apropriate answer
        try { Action = int.Parse(Console.ReadLine()); //getting their choice for this loop
            if (Action >= 1 && Action <=6) { break; }
        }
        catch {  }
    }

    ClumsyRand = rand.Next(1, 11); CriticalRand = rand.Next(1, (CriticalRanMax+1)); //Rolling for any random multipliers 
    if (ClumsyRand == 10) { CriticalMulti *= 2; } //This is the clumsy nature randomiser
    if (CriticalRand == 10) { CriticalMulti *= 2; } //This is the random critical randomiser
    ChaosPercent = rand.Next(1, 500)/1000;
    if (ChaosChance >= (rand.Next(1, 1000)/1000) && Chaos == true) { TurnStatChange(Action, (1-ChaosPercent)); TurnStatChange(rand.Next(1, 4), ChaosPercent); }

    if ((FailChance == false || ClumsyRand != 1) && Chaos == false) {
        TurnStatChange(Action, 1);
    }

    Pets[CurrentPet].Age += 0.1f; //Changing the pet's variables
    Pets[CurrentPet].Hunger += (10 * HungRedMulti);
    Pets[CurrentPet].Happiness += (10 * HapiRedMulti);
    Pets[CurrentPet].Intelegence -= (2 * InteRedMulti);
    if (Pets[CurrentPet].Intelegence < 0) { Pets[CurrentPet].Intelegence = 0; }
    Pets[CurrentPet].Fitness -= (5 * FitnRedMulti);
    if (Pets[CurrentPet].Fitness < 0) { Pets[CurrentPet].Fitness = 0; }
    CriticalMulti = 1;

    if (Pets[CurrentPet].Hunger > (100 + HungExtCapacity))
    {  //All potencial death equivlelent codes
        Console.Clear();
        Console.WriteLine(Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + " died due to hunger. Good job!"); //Informing the player of their mistake
        DeletePet();
    }
    if (Pets[CurrentPet].Happiness > 100)
    {
        Console.Clear();
        Console.WriteLine(Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + " left because you neglected them."); //Informing the player of their mistake
        DeletePet();
    }
    if (Pets[CurrentPet].Age >= 10) {
        Console.Clear();
        Console.WriteLine(Pets[CurrentPet].Name + " has did of old age and so his child will take over");
        CreateChild();
    }
}

void CreatePet()
{
    Console.WriteLine("Give the pet a name"); //Gets the pet's name from the user
    Pets[PetCount] = new Pet();
    Console.Write("First name: ");
    Pets[PetCount].Name = Console.ReadLine().ToString();
    Console.Write("Last name: ");
    Pets[PetCount].LastName = Console.ReadLine().ToString();
    PetCount++; //Logs the new pet to the count
    NatureSwitch();
    CurrentPet = PetCount-1;
}

void CreateChild()
{
    Console.Write("Give the decendent of " + Pets[CurrentPet].Name + " a name: ");
    Pets[CurrentPet].Name = Console.ReadLine();
    NatureRand = rand.Next(1, 21);
    if (NatureRand <= 3) { //A 3/20 chance for a child to have a different nature to the parent
        Pets[CurrentPet].Nature = Pets[CurrentPet].RandomiseNature();
        NatureSwitch();
    }

    //Used desmos's graphing calculator for the next part to get a proper look at the stat variation chances
    //Put in   x=\left(y-2.1\right)^{-7}+0.75\left\{x\ge0\right\}\left\{y\ge0.5\right\}\left\{x\le1\right\}\left\{y\le1\right\}
    //And      x=\left(y+1.2\right)^{-7}+0.688\left\{x\ge0\right\}\left\{y\ge0\right\}\left\{x\le1\right\}\left\{y\le0.5\right\}
    //To see the display of the variation used in this child stat distribution
    StatRand = rand.Next(0, 1000); //Generates a random number between 0 and 1 and applies it to the formula above
    StatRand = (StatRand + (Pets[CurrentPet].Generation * 10)) / (1000 + (Pets[CurrentPet].Generation*10)); //The longer you keep a family alive the more stats are likely
    if (StatRand >= 0.5) {                                                                                  //to go to the next generation
        StatRand = (float)(Math.Pow((StatRand - 2.1f),-7) + 0.75f);
    } else {  
        if (StatRand < 0.5) {
            StatRand = (float)(Math.Pow((StatRand + 1.2f),-7) + 0.688f);
        }
    }

    Pets[CurrentPet].Fitness *= StatRand; //Reduces the next generations stats by the random amount
    Pets[CurrentPet].Intelegence *= StatRand;
    Pets[CurrentPet].Generation++;
    Pets[CurrentPet].Age -= 10; //Resets the age for the new pet
}

void PetSwitch()
{
    if (PetCount != 0) { //Skips switching if you have no pets left
        Console.WriteLine("Please chose a pet: ");
        for (int i = 0; i <= (PetCount - 1); i++) { //Repeats through the list of pets cause I don't know how many there will be when this is called
            Console.WriteLine((i+1) + "." + Pets[i].Name + " " + Pets[i].LastName);
        }
        Console.Write("Your choice: ");
        while (true) { //Preventing going forward without an answer
            try { CurrentPet = int.Parse(Console.ReadLine()); //Getting the users input again
                if (CurrentPet <= PetCount && CurrentPet >= 0) { CurrentPet--;  break; }
            }
            catch {  }
        }
        NatureSwitch();
    } else {
        Console.WriteLine("You are out of pets so you need to make another to keep playing.");
        CreatePet(); //Forces you to make a new pet if their are none left
    }
}

void NatureSwitch()
{
    HungMulti = 1; HungRedMulti = 1; //Resets the nature variables so they can be applied again
    HapiMulti = 1; HapiRedMulti = 1;
    InteMulti = 1; InteRedMulti = 1;
    FitnMulti = 1; FitnRedMulti = 1;
    HungExtCapacity = 0; FailChance = false;
    CriticalRanMax = 10; PlayBoyActive = false;
    CurrentNature = ""; Chaos = false;

    switch (Pets[CurrentPet].Nature) { //Just applys the multipliers and changes needed for the current pet
        case 'L':
            CurrentNature = "Lethargic";
            FitnMulti = 0.8f; HapiRedMulti = 0.8f;
            break;
        case 'I':
            CurrentNature = "Intelegent";
            FitnMulti = 0.6f; InteMulti = 1.4f;
            break;
        case 'R':
            CurrentNature = "Rock brain";
            FitnMulti = 1.2f; InteMulti = 0.8f;
            break;
        case 'J':
            CurrentNature = "Joyful";
            HapiMulti = 1.1f; HapiRedMulti = 0.8f;
            InteMulti = 0.8f; FitnMulti = 0.8f;
            break;
        case 'H':
            CurrentNature = "Hoarder";
            HungExtCapacity = 20; HungRedMulti = 0.8f;
            HapiMulti = 0.8f; InteMulti = 0.8f;
            break;
        case 'C':
            CurrentNature = "Clumsy";
            FailChance = true;
            break;
        case 'B':
            CurrentNature = "Blessed";
            CriticalRanMax = 7;
            InteMulti = 0.9f; FitnMulti = 0.9f; HapiMulti = 0.9f;
            break;
        case 'P':
            CurrentNature = "PlayBoy";
            PlayBoyActive = true;
            break;
        case 'S':
            switch (Pets[CurrentPet].Spec) //Applies the apropriate stats for the Specialisation
            {
                case 1:
                    CurrentNature = "Specialised (Hung)";
                    HungMulti = 1.4f; HapiMulti = 0.9f;
                    InteMulti = 0.9f; FitnMulti = 0.9f;
                    break;
                case 2:
                    CurrentNature = "Specialised (Hapi)";
                    HungMulti = 0.9f; HapiMulti = 1.4f;
                    InteMulti = 0.9f; FitnMulti = 0.9f;
                    break;
                case 3:
                    CurrentNature = "Specialised (Intel)";
                    HungMulti = 0.9f; HapiMulti = 0.9f;
                    InteMulti = 1.4f; FitnMulti = 0.9f;
                    break;
                case 4:
                    CurrentNature = "Specialised (Fitns)";
                    HungMulti = 0.9f; HapiMulti = 0.9f;
                    InteMulti = 0.9f; FitnMulti = 1.4f;
                    break;
            }
            break;
        case 'K':
            CurrentNature = "Chaotic";
            Chaos = true; //Pandoras box
            break;

        default: Environment.Exit(0); break;
    }
}

void DeletePet()
{
    if ((CurrentPet + 1) != PetCount)
    { //Removing the potencial empty space from the array and overlaying the old pets info
        for (int x = (CurrentPet); x < (PetCount - 1); x++)
        {
            Pets[x] = Pets[x + 1];
        } //I leave the last entry duplicated because it would be replaced upon creating another pet
    }
    PetCount--;
    PetSwitch();
}

void TurnStatChange(int Act, float ChaosPer)
{
    switch (Act)
    { //Parsing their choice into something tangable (even if its just a line)
        case 1:
            Pets[CurrentPet].Hunger -= (30 * HungMulti * CriticalMulti * ChaosPer);
            if (Pets[CurrentPet].Hunger < 0) { Pets[CurrentPet].Hunger = 0; } //Hunger option
            break;
        case 2:
            Pets[CurrentPet].Happiness -= (50 * HapiMulti * CriticalMulti * ChaosPer);
            if (Pets[CurrentPet].Happiness < 0) { Pets[CurrentPet].Happiness = 0; } //Play option
            break;
        case 3:
            Pets[CurrentPet].Intelegence += (10 * InteMulti * CriticalMulti * ChaosPer); //Read option
            break;
        case 4:
            Pets[CurrentPet].Fitness += (30 * FitnMulti * CriticalMulti * ChaosPer); //Exersize option
            break;
        case 5:
            Console.Clear();
            Console.WriteLine(Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + " waves goodbye as you move onto another pet.");
            PetSwitch();
            break;
        case 6:
            Console.Clear();
            Console.WriteLine(Pets[CurrentPet].Name + " " + Pets[CurrentPet].LastName + " waves goodbye as you move onto another pet.");
            CreatePet();
            break;
        default:
            Console.WriteLine("Please choose an apropriate number."); //Preventing errors from incorrect input
            break;
    }
}

// --To Add--
// - Natures effecting stat loss/gain                               - Mosty complete
// - More stats like intelegence, fitness and happiness             - Done
// - More actions to go along with the new stats                    - Done
// - A better looking UI (using console.clear)                      - Done
// - Chance events like a critical cycle with extra gains           - Done
//   and losses and a slight variation in the changes for a pet
// - Might be able to make a form of progress/stat bar for the UI   - Done
// - Death of old age and kids that take some stats and have a      - Done
//   good chance to copy the nature of their parents
// - Fix the extra long hunger UI bug                               - 
// - Add the last 2 natures                                         - Done
// - Prevent the user switching specialisation after pet switch     - Done