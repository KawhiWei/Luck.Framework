using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Walnut.Dto.Environments
{
    public class SelectedItem
    {

        public SelectedItem(string value, string title)
        {
            Title = title;
            Value = value;
        }
        public string Value { get; private set; }
        public string Title { get; private set; }

   

    }
}
