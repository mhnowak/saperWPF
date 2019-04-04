using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    public class SquareBombsList : ObservableCollection<bool>
    {
        public SquareBombsList(int  t) : base()
        {
            for(int i=0; i < t; i++)
            {
                Add(false);
            }  
        }
    }
}
