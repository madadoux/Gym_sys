
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 


namespace GYM_system
{
    class GymSystem
    {
        public GymSystem(string _name , FileStream _memberFS , FileStream _staffFS)
        {
            this.Name = _name;
            this.memberFS = _memberFS;
            this.StaffFS = _staffFS; 
        }
        public string Name;
        FileStream memberFS, StaffFS; 

        public void Add(RecordableObject _s) {

            

                FileStream CurrFile =null ;


                if (_s.GetType() == typeof(Staff))
                   CurrFile = StaffFS;
                else if (_s.GetType() == typeof(Member))
                    CurrFile = memberFS; 


			//Creating new head 
                if (CurrFile.Length == 0)
                {
                    CurrFile.WriteByte((byte)0); 
			     }
			//gettingTHehead 
                CurrFile.Seek(0, SeekOrigin.Begin);
                int head = (int)CurrFile.ReadByte();

			//no deleted recs
			if (head == 0) {
                CurrFile.Seek(0, SeekOrigin.End);
                CurrFile.Write(_s.toByteArr(), 0, _s.toByteArr().Length); 
			} else {

				//move to last deleted rec

                CurrFile.Seek(head, SeekOrigin.Begin);
                int length = (int)CurrFile.ReadByte(); 

				//if last rec is fit
				if (length >= _s.RecLen) {
					byte[] chilld = new byte[2];

                    CurrFile.Read(chilld, 0, 2);
                    CurrFile.Seek(head, SeekOrigin.Begin);
                    CurrFile.ReadByte(); // ignoring the perv Length 
                    byte[] TT = _s.toByteArr();
                    CurrFile.Write(TT, 1, TT.Length - 1);
                    CurrFile.Seek(0, SeekOrigin.Begin);
                    CurrFile.WriteByte(chilld[1]); 

				} else {
                    long parentOffset = CurrFile.Position - 1;
					long currOffset= -1; 

					bool FoundFit = true;
					while (length < _s.RecLen){
						
						parentOffset = CurrFile.Position -1; // we read abyte(length)
						byte[] chilld = new byte[2];

                        CurrFile.Read(chilld, 0, 2);
						currOffset = (int)chilld [1]; 
						//wsl l25r el stack 
						if(currOffset ==0) 
						{
                            FoundFit = false; 
							break ;

						}
                        CurrFile.Seek(currOffset, SeekOrigin.Begin); 
						length = (int) CurrFile .ReadByte (); 


					} 

					if (FoundFit) {

                        CurrFile.Seek(currOffset, SeekOrigin.Begin); 
						length = (int)CurrFile.ReadByte (); 

						byte[] childofCurrent = new byte[2];
                        CurrFile.Read(childofCurrent, 0, 2);

                        CurrFile.Seek(parentOffset + 2, SeekOrigin.Begin);
                        CurrFile.WriteByte(childofCurrent[1]);
                        
                        CurrFile.Seek(currOffset, SeekOrigin.Begin);
                        CurrFile.ReadByte(); 
                        CurrFile.Write(_s.toByteArr(), 0, _s.toByteArr().Length);

					} else {
                        CurrFile.Seek(0, SeekOrigin.End);
                        CurrFile.Write(_s.toByteArr(), 0, _s.toByteArr().Length); 
					}

				}	

			}


		
            CurrFile.Flush(); 


        }
        public bool Update(string ID, RecordableObject _newData) {

            if (_newData.GetType() == typeof(Staff))
            {
                if (DeleteStaff(ID))
                {
                    Add(_newData);
                    return true;
                }
                else return false;
            }
            else if (_newData.GetType() == typeof(Member))
            {
                if (DeleteMember(ID))
                {
                    Add(_newData);
                    return true;
                }
                else return false;
            }
            else return false; 
        
        }



        public bool RestoreMember(string ID)
        {
            Member Curr = SearchForMember(ID,HelperFunc.RetriveMode.deleted).Value;
            if (Curr != null)
            {
                Member New = new Member(Curr.ID, Curr.Name, Curr.mem_type, Curr.MemStartDate, Curr.MemEndDate);
                memberFS.Seek(0, SeekOrigin.End);
                memberFS.Write(New.toByteArr(), 0, New.toByteArr().Length);
                return true;
            }
            else return false;

        }

        public bool RestoreStaff(string ID)
        {
             Staff  Curr = SearchForStaff(ID, HelperFunc.RetriveMode.deleted).Value;
            if (Curr != null)
            {
                Staff New = new Staff(Curr.ID, Curr.Name, Curr.jobType);
                StaffFS.Seek(0, SeekOrigin.End);
                StaffFS.Write(New.toByteArr(), 0, New.toByteArr().Length);
                return true;
               
            }
           return false;
        }

        public bool ExtenedMemberShip(string ID , int _monthCount)
        {

            Member Curr = SearchForMember(ID,HelperFunc.RetriveMode.notDeleted).Value;
            if (Curr != null)
            {
                DateTime newData = Curr.MemEndDate.AddMonths(_monthCount);

                Curr.MemEndDate = newData;

                if (Update(ID, Curr))
                    return true;
                else
                return false;
            }
            else 
                return false; 
           
        }
      public  KeyValuePair<long, Member> SearchForMember(string ID,HelperFunc.RetriveMode Show)
        {

            memberFS .Seek(1, SeekOrigin.Begin);
            while (memberFS.Position != memberFS.Length)
            {
                int length = (int)memberFS.ReadByte();
                byte[] tbyte = new byte[length];
                memberFS.Read(tbyte, 0, tbyte.Length);
                Member s = HelperFunc.ByteToMember(tbyte);
                if (s.ID == ID && s.deleted && (Show == HelperFunc.RetriveMode.deleted || Show == HelperFunc.RetriveMode.Both))
                    return new KeyValuePair<long, Member>(memberFS.Position - length, s);

                if (s.ID == ID && !s.deleted && (Show == HelperFunc.RetriveMode.notDeleted || Show == HelperFunc.RetriveMode.Both))
                    return new KeyValuePair<long, Member>(memberFS.Position - length, s); 

            }
            return new KeyValuePair<long, Member>(-1, null); 
            
        }
      public KeyValuePair<long, Staff> SearchForStaff(string ID, HelperFunc.RetriveMode Show)
        {

			StaffFS.Seek (1, SeekOrigin.Begin); 
			while (StaffFS.Position != StaffFS.Length ) {
                int length = (int)StaffFS.ReadByte();
				byte[] tbyte = new byte[length];  
				StaffFS.Read(tbyte, 0, tbyte.Length); 
				Staff s = HelperFunc.ByteToStaff (tbyte); 
				if (s.ID == ID && s.deleted && (Show == HelperFunc.RetriveMode.deleted || Show == HelperFunc.RetriveMode.Both)) 
					return new KeyValuePair<long,Staff>(StaffFS.Position - length , s); 
			
				if ( s.ID == ID &&!s.deleted && (Show == HelperFunc.RetriveMode.notDeleted || Show == HelperFunc.RetriveMode.Both))
                    return new KeyValuePair<long, Staff>(StaffFS.Position - length, s); 

			}
			return new KeyValuePair<long , Staff>(-1 , null)   ; 

           
        }
        public bool DeleteMember(string ID)
        {
            if (memberFS.Length != 0)
            {
                memberFS.Seek(0, SeekOrigin.Begin);// file begin 
                int head = memberFS.ReadByte();
                KeyValuePair<long, Member> Where = SearchForMember(ID,HelperFunc.RetriveMode.notDeleted);
                if (Where.Value != null && !Where.Value.deleted)
                {
                    memberFS.Seek(Where.Key, SeekOrigin.Begin);
                    memberFS.WriteByte((byte)'^');
                    memberFS.WriteByte((byte)head);
                    memberFS.Seek(0, SeekOrigin.Begin);
                    memberFS.WriteByte((byte)(Where.Key - 1));
                    memberFS.Flush();
                    return true;
                }
                else
                    return false;
            } return false;
        }


        public bool DeleteStaff(string ID) { 
			if (StaffFS.Length != 0) {
				StaffFS.Seek (0, SeekOrigin.Begin);
				int head = StaffFS.ReadByte (); 
				KeyValuePair<long , Staff> Where = SearchForStaff (ID ,HelperFunc.RetriveMode.notDeleted);
				if (Where.Value != null   ) {
					StaffFS.Seek (Where.Key, SeekOrigin.Begin);
					StaffFS.WriteByte ((byte)'^');
					StaffFS.WriteByte ((byte)head);
					StaffFS.Seek (0, SeekOrigin.Begin);
					StaffFS.WriteByte ((byte)(Where.Key-1) );
					StaffFS.Flush ();	
					return true; 
				} else
					return false;
			}return false; 
		}

		public List<Staff> get_Staff(HelperFunc.RetriveMode Show){  

			List<Staff> staff = new List<Staff> ();
            if (StaffFS.Length != 0)
            {
                StaffFS.Seek(1, SeekOrigin.Begin);

                while (StaffFS.Position != StaffFS.Length)
                {

                    int length = (int)StaffFS.ReadByte();

                    byte[] tbyte = new byte[length];
                    StaffFS.Read(tbyte, 0, length);
                    Staff s = HelperFunc.ByteToStaff(tbyte);
                    if (s.deleted && (Show == HelperFunc.RetriveMode.deleted || Show == HelperFunc.RetriveMode.Both)) 
                        staff.Add(s);
                     if (!s.deleted && (Show == HelperFunc.RetriveMode.notDeleted || Show == HelperFunc.RetriveMode.Both) )
                        staff.Add(s); 

                }
            }

			return staff; 
		}
        public List<Member> get_Members(HelperFunc.RetriveMode Show)
        {
            List<Member> members = new List<Member>();
            if (   memberFS  .Length != 0)
            {
                memberFS.Seek(1, SeekOrigin.Begin);

                while (memberFS.Position != memberFS.Length)
                {

                    int length = (int)memberFS.ReadByte();

                    byte[] tbyte = new byte[length];
                    memberFS.Read(tbyte, 0, length);
                    Member s = HelperFunc.ByteToMember(tbyte);
                    if (s.deleted && (Show == HelperFunc.RetriveMode.deleted || Show == HelperFunc.RetriveMode.Both))
                        members.Add(s);
                     if (!s.deleted && (Show == HelperFunc.RetriveMode.notDeleted || Show == HelperFunc.RetriveMode.Both))
                        members.Add(s); 

                }
            }

			return members; 
        
        }



    }
}
