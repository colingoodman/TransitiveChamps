using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitiveChamps
{
    class Team
    {
        public string name;
        public List<Team> losses = new List<Team>();
        public bool inTree = false;

        public Team(string input)
        {
            name = input;
        }

        public void AddBeaten(Team input)
        {
            //need to ensure that team isn't already in the losses list
            if(losses.Count == 0)
            {
                losses.Add(input);
            }
            else
            {
                for (int i = 0; i < losses.Count; i++)
                {
                    if (losses[i].ToString() != input.ToString())
                    {
                        losses.Add(input);
                    }
                }
            }
            
            
        }

        public string[] ReturnLosses()
        {
            string[] output = new string[losses.Count];
            for(int i = 0; i < losses.Count; i++)
            {
                output[i] = losses[i].ToString();
            }
            return output;
        }

        override public string ToString()
        {
            return name;
        }
    }
}
