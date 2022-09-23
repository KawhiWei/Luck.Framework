namespace Luck.Framework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectOptionList
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        public SelectOptionList(string title,string value)
        {
            Title = title;
            Value = value;

        }


        /// <summary>
        /// 
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; private set; }

   
    }
}
