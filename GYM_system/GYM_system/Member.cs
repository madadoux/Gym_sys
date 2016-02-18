using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM_system
{
     class Member : RecordableObject
    {
        public bool deleted; 
        public Member( string _ID , string _name , HelperFunc.MemType _type  , DateTime _memStartDate , DateTime _memEndDate )
        {
            ID = _ID;
            Name = _name;
            mem_type = _type;
            MemStartDate = _memStartDate;
            MemEndDate = _memEndDate;
            RecLen = 2 + ID.Length + 1 + Name.Length + 1 + 1 + 1 + MemStartDate.ToShortDateString().Length + 1 + MemEndDate.ToShortDateString().Length+1; 
           
        }
        public Member()
        {

        }

		public override byte[] toByteArr()
        {
            byte[] Tbyte = new byte[1 + RecLen];
            Tbyte[0] = (byte)RecLen;
            Tbyte[1] = (byte)'\0';
            Tbyte[2] = (byte)'\0';
			string tstring = "";
            tstring += ID + "@" + Name + "@" + (char)mem_type + "@" + MemStartDate.ToShortDateString() + "@" + MemEndDate.ToShortDateString() + "@";

			byte[] tarr = HelperFunc.tobyteArr (tstring);
			for (int i = 3; i <tarr.Length+3 ; i++) {
				Tbyte [i] = tarr [i-3]; 
			}
			return Tbyte; 
        }

        //fields
        public string ID { get; set; }
        public  string Name { get; set; }
        public HelperFunc.MemType mem_type { get; set;  }
          public   DateTime MemStartDate { set; get;  }
     public   DateTime MemEndDate { set; get; }
     
    }
}
