using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM_system
{
    class Staff :RecordableObject
    {
        public string  ID { get; set; }
        public string Name { get; set; }

		public bool deleted = false;
       
        public HelperFunc.JopType jobType { set; get;  }
        public Staff(string _ID , string _name , HelperFunc.JopType _type )
        {
            ID = _ID;
            Name = _name; 
            jobType = _type;
			RecLen = 2 + ID.Length + 1 + Name.Length+1 + 1+1; 

        }
        public Staff()
        {

        }

		public   override byte[] toByteArr()
        {
			byte[] Tbyte = new byte[1 + RecLen];
			Tbyte[0] = (byte)RecLen;
			Tbyte[1] = (byte)'\0';
			Tbyte[2] = (byte)'\0';
			string tstring = ""; 
			tstring += ID + "*" + Name + "*" + (char)jobType+'*'; 

			byte[] tarr = HelperFunc.tobyteArr (tstring);
			for (int i = 3; i <tarr.Length+3 ; i++) {
				Tbyte [i] = tarr [i-3]; 

			}
			return Tbyte; 
        }

    }
}
