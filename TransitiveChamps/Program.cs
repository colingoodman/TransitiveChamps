/* Colin Goodman - April 2018
 * TransitiveChamps Project
 * Learn more about this project on Github:
 * https://github.com/colingoodman/TransitiveChamps
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransitiveChamps
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read input file and break it up
            List<string[]> lines = new List<string[]>();
            lines = LoadFile(lines);

            //Create Team objects for every unique team found in the input file
            Dictionary<string, Team> teams = new Dictionary<string, Team>();
            teams = PopulateTeams(lines);

            //Create the tree and determine the # of transitive nonwinners
            Tree master = QueuePopulate(teams);
            int losers = NumLosers(teams);

            //Tell the user what the program found
            Console.WriteLine("Total teams: " + teams.Count);
            Console.WriteLine("Number of teams that were transitive winners: " + (teams.Count-losers));
            Console.WriteLine("Number of teams that were not transitive winners: " + losers);

            Console.ReadLine();
        }

        public static int NumLosers(Dictionary<string, Team> teams)
        {
            int count = 0;
            
            foreach(KeyValuePair<string, Team> entry in teams)
            {
                // do something with entry.Value or entry.Key
                if(teams[entry.Key].inTree == false)
                {
                    count++;
                }
            }

            return count;
        }

        public static Tree QueuePopulate(Dictionary<string, Team> teams)
        {
            Console.WriteLine("Now populating the tree.");

            Queue<Team> populate = new Queue<Team>();
            Tree result = new Tree(teams["Villanova"]);
            populate.Enqueue(teams["Villanova"]);

            //ALG
            //while the queue is not empty,
            //dequeue, add children to queue and tree

            while(populate.Count > 0)
            {
                Team temp = populate.Dequeue();
                Tree target = result.Traverse(temp);

                for(int i = 0; i < temp.losses.Count; i++)
                {
                    if(temp.losses[i].inTree == false)
                    {
                        populate.Enqueue(temp.losses[i]); //add child to queue
                        target.AddChild(new Tree(temp.losses[i])); //add child to tree
                        temp.losses[i].inTree = true;
                        
                    }
                }
            }

            return result;
        }
        
        //dont use
        public static Tree RecursivePopulate(Dictionary<string, Team> teams, Team cur)
        {
            Tree thisTree = new Tree(cur);

            //Console.WriteLine("Recursive call");

            //base
            //if this team has not beaten anyone
            if(teams[cur.ToString()].losses.Count == 0)
            {
                Tree temp = new Tree(cur);
                cur.inTree = true;
                return temp;
            }

            //recursion

            for(int i = 0; i < teams[cur.ToString()].losses.Count; i++)
            {
                if(teams[cur.ToString()].losses[i].inTree == false)
                {//if this team has lost to someone not in the list
                    //then add that team to the fucking tree w/ recursion
                    thisTree.AddChild(RecursivePopulate(teams, teams[cur.ToString()].losses[i]));
                    cur.inTree = true;
                }
            }

            return thisTree;
        }

        public static Dictionary<string,Team> PopulateTeams(List<string[]> lines)
        {
            Dictionary<string, Team> output = new Dictionary<string, Team>();
            double PROCESS_AMT = lines.Count;
            Console.WriteLine("Now processing games from games.txt");

            //traverse lines and add all teams to the team list only once
            for(int i = 0; i < PROCESS_AMT; i++)
            {//i used two for loops here in case a team only ever showed up on the same column somehow
                
                double perc = i / PROCESS_AMT;
                if(perc*100 % 2 == 0)
                {
                    Console.WriteLine(perc * 100 + "% of games processed.");
                }

                //first, check to see if either team has not been already added to the dictionary
                string tempOne = lines[i][1];
                Team newOne = new Team(tempOne);
                string tempTwo = lines[i][3];
                Team newTwo = new Team(tempTwo);

                try
                {
                    output.Add(tempOne, newOne);
                }
                catch
                {
                }
                try
                {
                    output.Add(tempTwo, newTwo);
                }
                catch
                {
                }

                //now, add the winner to the loser's object
                if(Int32.Parse(lines[i][2]) > Int32.Parse(lines[i][4]))
                {//first team won
                    output[lines[i][3]].AddBeaten(output[lines[i][1]]);
                }
                else
                {//second tema won
                    output[lines[i][1]].AddBeaten(output[lines[i][3]]);
                }
            }

            return output;
        }

        public static List<string[]> LoadFile(List<string[]> file)
        {
            using (StreamReader sr = new StreamReader("games.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] input = line.Split(' ');
                    file.Add(FileFormat(input));
                }
            }

            Console.WriteLine("Loaded 'games.txt'");
            return file;
        }

        public static string[] FileFormat(string[] whitespace)
        {
            List<string> temp = new List<string>();
            string[] proper = new string[5];

            //this loop clears out all the extra whitespace from the input file
            for (int i = 0; i < whitespace.Length; i++)
            {
                if (whitespace[i] != "")
                {
                    temp.Add(whitespace[i]);
                }
            }

            //add dates
            proper[0] = temp[0];

            //some schools have spaces in their name == pain in ass
            //also there's probably a better way to handle this
            int school = 1;
            for (int j = 1; j < temp.Count; j++)
            {
                if (Char.IsNumber(temp[j][0]))
                {
                    proper[school + 1] = temp[j];
                    if (school == 3)
                    {
                        j = temp.Count;
                    }
                    else
                    {
                        school += 2;
                    }
                }
                else
                {
                    proper[school] = proper[school] + "_" + temp[j];
                }
            }

            //remove extra _ in front of school name
            proper[1] = proper[1].Remove(0, 1);
            proper[3] = proper[3].Remove(0, 1);

            //remove @'s
            for (int i = 1; i < proper.Length; i++)
            {
                if (proper[i].Contains('@'))
                {
                    for (int j = 0; j < proper[i].Length; j++)
                    {
                        if (proper[i][j] == '@')
                        {
                            proper[i] = proper[i].Remove(j, 1);
                        }
                    }
                }
            }

            return proper;
        }
    }
}
