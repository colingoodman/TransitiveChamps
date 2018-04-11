using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitiveChamps
{
    class Tree
    {
        public Team team;
        public List<Tree> children = new List<Tree>();

        public Tree(Team name)
        {
            team = name;
        }

        public void AddChild(Tree name)
        {
            children.Add(name);
        }

        public List<Team> GetChildren(List<Team> input)
        {
            if(children.Count == 0)
            {
                input.Add(team);
                return input;
            }
            else
            {
                for(int i = 0; i < children.Count; i++)
                {
                    List<Team> temp = children[i].GetChildren(input);
                    for (int j = 0; j < temp.Count; j++)
                    {
                        input.Add(temp[j]);
                    }
                    input.Add(team);
                }
                return input;
            }
        }
    }
}
