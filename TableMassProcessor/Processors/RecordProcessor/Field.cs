using System;
using System.Collections.Generic;
using System.Text;

namespace RecordProcessor
{
    public class Field
    {
        private string _name;
        private bool _isRequired;

        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }
        private List<string> _aliases = new List<string>();
        
        /// <summary>
        /// Constructor with no aliases
        /// </summary>
        /// <param name="name"></param>
        public Field(string name, bool isrequired)
        {
            _name = name;
            _isRequired = isrequired;
        }
        /// <summary>
        /// Constructor with aliases
        /// </summary>
        /// <param name="name"></param>
        /// <param name="aliases"></param>
        public Field(string name, bool isrequired, string[] aliases)
        { 
            _name = name;
            _isRequired = isrequired;
            
            //Add case insensitive
            foreach (string alias in aliases)
            {
                _aliases.Add(alias.ToLower());
            }
        }

        /// <summary>
        /// Field name to process
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Field alias to autolink
        /// </summary>
        public List<string> Aliases
        {
            get { return _aliases; }
            set { _aliases = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
