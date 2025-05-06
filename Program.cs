using System;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Threading;
//Andrew Wells 801378549
namespace TextBasedRPG
{
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public Item(string name, string description, int quantity = 1)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
        }
    }
    class Inventory
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public void AddItem(Item newItem)
        {
            var existingItem = Items.Find(i => i.Name == newItem.Name);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                Items.Add(newItem);
            }
        }
        public void RemoveItem(string itemName, int quantity = 1)
        {
            var itemToRemove = Items.Find(i => i.Name == itemName);
            if (itemToRemove != null)
            {
                itemToRemove.Quantity -= quantity;
                if (itemToRemove.Quantity <= 0)
                {
                    Items.Remove(itemToRemove);
                }
            }
        }
        public void ShowInventory()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine("Your inventory is empty");
                return;
            }
            Console.WriteLine("\nInventory:");
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"{i+1}. {Items[i].Name} x{Items[i].Quantity}");
                Console.WriteLine($"   {Items[i].Description}");
            }
        }
        public void UseItem(Player player, string itemName)
        {
            var item = Items.Find(i => i.Name == itemName);
            if (item == null)
            {
                Console.WriteLine($"You don't have {itemName} in your inventory.");
                return;
            }
            Console.WriteLine($"You use {itemName}.");
            RemoveItem(itemName);


        }

    }
    class Player
    {
        public int StoryProgress { get; set; } = 0;
        public Inventory Inventory { get; set; } = new Inventory();
        public string _name { get; set; }
        public string _class { get; set; }
        public int _hp { get; set; }

        public int _level { get; set; }

        public int _experience { get; set; }

        public double _threshold { get; set; }

        public int _abilityPower { get; set; }

        public int _defense { get; set; }

        public int _mana { get; set; }

        public int _maxHP { get; set; }

        public int _maxMana { get; set; }
        public void ShowStats()
        {
            Console.WriteLine("Current Stats");
            Console.WriteLine($"Name: {_name}");
            Console.WriteLine($"Class: {_class}");
            Console.WriteLine($"Level: {_level}");
            Console.WriteLine($"HP: {_hp}");
            Console.WriteLine($"Mana: {_mana}");
            Console.WriteLine($"Ability Power: {_abilityPower}");
            Console.WriteLine($"Armor: {_defense}");

        }
        public List<Ability> Abilities { get; set; } = new List<Ability>();

        public Player()
        {
            _name = "";
            _class = "Undecided"; //Default
            _hp = 100;
            _maxHP = 100;
            _mana = 250;
            _maxMana = 250;
            _level = 1;
            _experience = 0;
            _threshold = 100;
            _abilityPower = 5;
            _defense = 10;

            Inventory = new Inventory();

            

        }
        public void SetName(string _setName)
        {
            _name = _setName;
        }
        public void SetClass(string _playerClass)
        {
            Abilities.Clear();
            _class = _playerClass;

            switch (_playerClass)
            {
                case "Fire Mage":
                    _hp = 50;
                    _maxHP = 50;
                    _abilityPower = 5;
                    _defense = 4;
                    _mana = 100;
                    _maxMana = 100;
                    Abilities.Add(new Ability("Fire blast", 10, 5));
                    Abilities.Add(new Ability("Wand", 5, 0));
                    Abilities.Add(new Ability("Fire Nova", 25, 50));
                    break;
                case "Ice Mage":
                    _hp = 75;
                    _maxHP = 75;
                    _abilityPower = 3;
                    _defense = 10;
                    _mana = 100;
                    _maxMana = 100;
                    Abilities.Add(new Ability("Ice needle", 15, 15));
                    Abilities.Add(new Ability("Ice reflect", 10, 10));
                    Abilities.Add(new Ability("Ice Cone", 30, 25));
                    break;
                case "Death Mage":
                    _hp = 60;
                    _maxHP = 60;
                    _abilityPower = 7;
                    _defense = 5;
                    _mana = 200;
                    _maxMana = 200;
                    Abilities.Add(new Ability("Death strike", 50, 75));
                    Abilities.Add(new Ability("Life Dragin", 25, 40));
                    break;
            }
        }

        public void ShowAbilities()
        {
            Console.WriteLine("\nAbilities:");
            for (int i = 0; i < Abilities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Abilities[i].Name} - Cost: {Abilities[i].ManaCost} MP");
            }
        }
        public void AddExperience(int xp)
        {
            _experience += xp;
            Console.WriteLine($"You have earned {xp} experience! Total xp: {_experience}");

            while (_experience >= _threshold)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            _level++;
            _experience -= (int)_threshold;
            _threshold *= 1.25;
            Console.WriteLine($"You have leveled up! You are now level {_level}");
            switch (_class)
            {
                case "Fire Mage":
                    
                    _maxHP += 5;
                    _hp = _maxHP;
                    _abilityPower += 3;
                    _defense += 1;
                    _maxMana += 25;
                    _mana = _maxMana;
                    break;
                case "Ice Mage":
                    _maxHP += 15;
                    _hp = _maxHP;
                    _abilityPower += 2;
                    _defense += 3;
                    _maxMana += 15;
                    _mana = _maxMana;
                    break;
                case "Death Mage":
                    _maxHP += 20;
                    _hp = _maxHP;
                    _abilityPower += 1;
                    _defense += 2;
                    _maxMana += 20;
                    _mana = _maxMana;
                    break;

            }
        }
        public void Reset()
        {
            _name = "";
            _class = "";
            _hp = 100;
            _experience = 0;
            _threshold = 100;

        }
    }
    class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }

        public Ability(string name, int damage, int manaCost)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
        }
    }
    class Program
    {
        static string[] storySegments =
        {
            "The town of Ashenhail is in the middle of a desert but along the outer edges of the desert is " +
                "a bright green and youthful forest, full of animals of different kinds and plants of different kinds. " +
                "The town of Ashenhail has an unnatural phenomenon that has yet to be explained. When it “rains” it rains in the forest, not in the desert. " +
                "When it “snows” it snows in the desert, not in the forest. \r\n",
            "The town is a peaceful place to live, created in 1100 CR. (1100 years after creation of the world) " +
                "The cobblestone roads, paving the way to a future. The town, run by knowledge. " +
                "Along Rickety Way Street, on your right, you have everything food related, the sweet smells of " +
                "bread products being baked, the children running into candy shops, " +
                "and \tthe adults relaxing in the finest restaurants for the evening dates.",
            "The second most popular attraction to everyone is the bakery, “Yeasts Feasts.” The science of bread and other yeast infected products are no secret. " +
                "In fact people love the importance of science, but most importantly they love Magyk. On your left, everything needed.",
            "The sounds of a tailor grunting every time a string is plucked, the people walking out with pre-made foods, and children running around trying to find the best of the best toys and clothes. At the end of Rickety way, the road splits off into two, left and right, around a mile radius hill.",
            "The hill is covered in grass, containing everything from berry bushes to redwood trees. After the road finishes circling the hill, the most impressive, attracts the most, and built the highest, is the Health, Magyk, and Science Academy. The walls of the Academy were built with marble, and engraved in them are the stories of our ancestors.",
            "The travels: through plains of water, terrains of white, and mountains of never-ending green; The foundation of the city, the building up with a hundred people that traveled on horses, waving their swords a hole within the blade of the swords to contain the powers of Magyk, enchanting the horses to move faster; The life-like Magyk that changed the hill from a sand dune to grass, and grew in size; sprouting plants and trees. As the academy builds, more houses are built, and more people come from all over. Finally, next to the grand doors of the Academy, a plaque. Etched into the plaque is the completion date of the town. 1204 CR. \r\n"

        };
        
        static void Main(string[] args)
        {
            AnimateText("Welcome to the World of Ashenhail!");
            Player player = new Player();
            StartGame(player);
        }

        static void StartGame(Player player)
        {

            AnimateText("Enter your name Adventurer!: ");

            string _playerName = Console.ReadLine();
            player.SetName(_playerName);

            AnimateText("Your name is " + player._name);
            DisplayMenu(player);


        }
        static void Combat(Player player)
        {
           
            Random rand = new Random();
            string[] enemyTypes = { "Goblin", "Bandit", "Wild Beast", "Rat" };
            string enemyType = enemyTypes[rand.Next(enemyTypes.Length)];

            int enemyHP = 30 + (player._level * 10);
            int enemyAttack = 5 + (player._level * 2);
            int enemyDefense = 2 + player._level;

            Console.WriteLine($"\nA wild {enemyType} appears! (HP: {enemyHP}, Attack: {enemyAttack}, Defense: {enemyDefense})");

            while (enemyHP > 0 && player._hp > 0)
            {
                Console.WriteLine($"\n{player._name} - HP: {player._hp}/{player._maxHP} | MP: {player._mana}/{player._maxMana}");
                Console.WriteLine($"{enemyType} - HP: {enemyHP}");
                Console.WriteLine("Choose your action:");
                Console.WriteLine("1. Basic Attack");

                
                for (int i = 0; i < player.Abilities.Count; i++)
                {
                    Console.WriteLine($"{i + 2}. {player.Abilities[i].Name} ({player.Abilities[i].ManaCost} MP) - {player.Abilities[i].Damage} damage");
                }

                string input = Console.ReadLine();

                if (input == "1")
                {
                   
                    int damage = Math.Max(5 + player._abilityPower - enemyDefense, 1);
                    enemyHP -= damage;
                    Console.WriteLine($"You attack the {enemyType} for {damage} damage!");
                }
                else if (int.TryParse(input, out int abilityIndex) && abilityIndex > 1 && abilityIndex <= player.Abilities.Count + 1)
                {
                    
                    Ability selectedAbility = player.Abilities[abilityIndex - 2];

                    if (player._mana >= selectedAbility.ManaCost)
                    {
                        player._mana -= selectedAbility.ManaCost;
                        int damage = Math.Max(selectedAbility.Damage + player._abilityPower - enemyDefense, 1);
                        enemyHP -= damage;

                       
                        switch (selectedAbility.Name)
                        {
                            case "Life Dragin":
                                int healAmount = damage / 2;
                                player._hp = Math.Min(player._hp + healAmount, player._maxHP);
                                Console.WriteLine($"You drain life from the {enemyType}! Dealt {damage} damage and healed {healAmount} HP!");
                                break;
                            case "Ice reflect":
                                player._defense += 2;
                                Console.WriteLine($"Your defense increases! You deal {damage} damage to the {enemyType}!");
                                break;
                            default:
                                Console.WriteLine($"You use {selectedAbility.Name} for {damage} damage!");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        continue; 
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

               
                if (enemyHP > 0)
                {
                    int enemyDamage = Math.Max(enemyAttack - player._defense, 1);
                    player._hp -= enemyDamage;
                    Console.WriteLine($"The {enemyType} attacks you for {enemyDamage} damage!");

                   
                    if (player.Abilities.Exists(a => a.Name == "Ice reflect"))
                    {
                        player._defense -= 2;
                    }
                }
            }

            if (player._hp <= 0)
            {
                Console.WriteLine("\nYou have been defeated...");
                player._hp = player._maxHP; //reset hp
            }
            else
            {
                int xpEarned = 25 + (player._level * 5);
                Console.WriteLine($"\nYou defeated the {enemyType}! Gained {xpEarned} XP.");
                player.AddExperience(xpEarned);

                
                player._hp = Math.Min(player._hp + 10, player._maxHP);
                player._mana = Math.Min(player._mana + 15, player._maxMana);
            }
        }
        static void DisplayMenu(Player _player)
        {

            AnimateText("Choose your class Adventurer!: ");
            AnimateText("1. Fire Mage");
            AnimateText("2. Ice Mage");
            AnimateText("3. Death Mage");
            string _playerClass = Console.ReadLine();

            switch (_playerClass)
            {
                case "1":
                    _player.SetClass("Fire Mage");
                    break;
                case "2":
                    _player.SetClass("Ice Mage");
                    break;
                case "3":
                    _player.SetClass("Death Mage");
                    break;
                default:
                    AnimateText("You must pick a choice!");
                    return;
            }

            AnimateText("You have chosen! " + _player._class + " Is this correct? (Y/N/R): ");
            string _confirm = Console.ReadLine().ToLower();

            if (_confirm == "y")
            {
                AnimateText("You are " + _player._class);
                ContinueGame(_player);

            }
            else if (_confirm == "r")
            {
                _player.Reset();
                StartGame(_player);
            }
            else
            {
                AnimateText("You must pick a choice! ");
                DisplayMenu(_player);
            }
        }

        static void AnimateText(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);
                Thread.Sleep(25);
            }
            Console.WriteLine();
        }
     
        static void ShowStory(Player player)
        {
            if (player.StoryProgress < storySegments.Length)
            {
                AnimateText(storySegments[player.StoryProgress]);
                player.StoryProgress++;

                if (player.StoryProgress < storySegments.Length)
                {
                    Console.WriteLine("\nPress any key");
                }
                else
                {
                    Console.WriteLine("\n(You've Reached the end of the current story)");
                }
            }
            else
            {
                Console.WriteLine("You've heard all the available story segments for now");
                Console.WriteLine("Maybe more will be revealed as you progress..");
            }
            Console.ReadKey();
        }

        static void ContinueGame(Player player)
        {
            while (true)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. View Stats");
                Console.WriteLine("2. View Abilities");
                Console.WriteLine("3. View Inventory");
                Console.WriteLine("4. Continue");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.ShowStats();
                        break;
                    case "2":
                        player.ShowAbilities();
                        break;
                    case "3":
                        player.Inventory.ShowInventory();
                       break;
                    case "4":
                        break;
                }
                Console.WriteLine("Choose a scenario");
                Console.WriteLine("1. Story");
                Console.WriteLine("2. Combat");
                Console.WriteLine("3. Explore");
                string scenario = Console.ReadLine();

                switch (scenario)
                {
                    case "1":
                        ShowStory(player);
                        break;
                    case "2":
                        Combat(player);
                       
                        break;
                    case "3":
                        Explore(player);
                        break;
                        

                }
              static void InventoryMenu(Player player)
                {
                    if (player.Inventory.Items.Count == 0) return;
                    Console.WriteLine("\nChoose an item to use (number), or 0 to exit:");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= player.Inventory.Items.Count)
                    {
                        Item selectedItem = player.Inventory.Items[itemIndex - 1];
                        player.Inventory.UseItem(player, selectedItem.Name);
                    }
                }
              static void Explore(Player player)
                {
                    Random rand = new Random();
                    int encounter = rand.Next(1, 5);
                    int goldAmount = rand.Next(1, 5);

                    if (encounter >= 1)
                    {
                        Console.WriteLine("You found an item!");

                        Item[] possibleItems =
                        {
                            new Item("Silver Key", "Key to open your room in the dorm", 1),
                            new Item("Book", "A simple story", 1),
                            new Item("Rusty nail", "Could be dangerous", 1),
                            new Item("Copper", "used as currency", goldAmount)



                        };
                        Item foundItem = possibleItems[rand.Next(possibleItems.Length)];
                        player.Inventory.AddItem(foundItem);
                        Console.WriteLine($"You found: {foundItem.Name}-{foundItem.Description}");
                    }
                    else
                    {
                        Console.WriteLine("You browse the area but find nothing important");
                    }
                }


            }


        }
      
    }
  
}









