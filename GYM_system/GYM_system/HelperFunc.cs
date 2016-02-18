using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM_system
{
    class HelperFunc
    {
        public enum MemType { Golden, Silver, VIP };
        public enum JopType { SeniorCoach, JuniorCoach, Secretary }
    public    enum RetriveMode { deleted, notDeleted, Both } ;


    public byte[] Encode(byte[] arr)
    {
        byte[] Encoded = new byte[arr.Length];
        return Encoded; 
    }



    public byte[] Decode(byte[] arr)
    {
        byte[] Decoded = new byte[arr.Length];
        return Decoded;
    }








       public static byte[] tobyteArr(string s)
        {
            byte[] Arr = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                Arr[i] = (byte)s[i];
            }
            return Arr;
        }

		public static Staff ByteToStaff (byte[] FromByte){
			string rec = ByteToString (FromByte); 
			Staff s = new Staff (); 
			string[] fields = rec.Split ('*'); 

			if (fields [0] [0] == '^')
				s.deleted = true; 
			char[] tid = new char[fields [0].Length - 2]; 
			fields [0].CopyTo (2, tid, 0, tid.Length); 
			s.ID = new string(tid) ; 
			s.Name = fields [1];
			s.jobType = (HelperFunc.JopType)fields [2][0];
			s.RecLen = FromByte.Length; 
			return s; 
            
			}

		public static Member ByteToMember (byte[] FromByte){
			string rec = ByteToString (FromByte); 
			Member s = new Member (); 

			string[] fields = rec.Split ('@');

            if (fields[0][0] == '^')
                s.deleted = true; 

			char[] tid = new char[fields [0].Length - 2]; 
			fields [0].CopyTo (2, tid, 0, tid.Length); 
			s.ID = new string(tid) ; 
			s.Name = fields [1];
			s.mem_type = (HelperFunc.MemType)fields [2][0];
			s.MemStartDate = HelperFunc.stringToDateTime( fields [3]);
			s.MemEndDate = HelperFunc.stringToDateTime( fields [4]);
            s.RecLen = FromByte.Length; 
			return s; 

		}

		public static	DateTime stringToDateTime(string  s) {
			string[] msg = s.Split ('/');
			return new DateTime (int.Parse (msg [2]),int.Parse (msg [1]), int.Parse (msg [0])); 
		}


		public static	string ByteToString(byte[] tarr) {
			string msg = ""; 

			for (int i = 0; i < tarr.Length; i++) {
				msg += (char)tarr [i]; 
			}
			return msg; 
		}


		}

    }

