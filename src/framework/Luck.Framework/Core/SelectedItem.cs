namespace Luck.Framework.Core
{
    public class SelectOptionList
    {

        public SelectOptionList(string title,string value)
        {
            Title = title;
            Value = value;

        }


        public string Title { get; private set; }

        public string Value { get; private set; }

   
    }
}
