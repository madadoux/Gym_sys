using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GYM_system
{
    public partial class Form1 : Form
    {
        FileStream memberFS, StaffFS;
        bool HasAform = false; 
        List<string> jobs = new List<string>();
        List<string> memberships = new List<string>();
        GymSystem g1;
        uint BTNindex = 0;
        bool Update = false; 
        public Form1()
        {

            InitializeComponent();
   
        }

        ~Form1(){
            memberFS.Close();
            StaffFS.Close();


        }

        void SetUpLayOut()
        {
            jobs.Add("Secretary");
            jobs.Add("JuniorCaoch");
            jobs.Add("SeniorCoach");

            memberships.Add("Golden");
            memberships.Add("Silver");
            memberships.Add("VIP");
            memShip_selector.DataSource = memberships;
            jop_selector.DataSource = jobs; 
            radioButton1.Checked = true;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            SetUpLayOut(); 

          memberFS = new FileStream("members.txt", FileMode.OpenOrCreate);
          StaffFS = new FileStream("staff.txt", FileMode.OpenOrCreate);
          g1 = new GymSystem("DUEX_Gym", memberFS, StaffFS); 

          Add_member_box.Enabled = false;
          Add_member_box.Visible = false;
          Add_staff_box.Enabled = false;
          Add_staff_box.Visible = false;

          this.Text = g1.Name; 
          dataGridView1.ReadOnly = true;
        }


        void UpdateGridView( HelperFunc.RetriveMode RM)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            if (radioButton1.Checked)
            {
                dataGridView1.Columns.Add("IDC", "ID");
                dataGridView1.Columns.Add("NAMEC", "NAME");
                dataGridView1.Columns.Add("JTC", "MemberType");
                dataGridView1.Columns.Add("SDC", "StartDate");
                dataGridView1.Columns.Add("EDC", "EndDate");

                int RowInd = 0; 
                var tt = g1.get_Members(RM);
                label8.Text = tt.Count.ToString();
                foreach (var rec in tt)
                {

                    dataGridView1.Rows.Add(rec.ID, rec.Name, (HelperFunc.MemType)rec.mem_type, rec.MemStartDate.ToShortDateString(), rec.MemEndDate.ToShortDateString());

                    if ( RM == HelperFunc.RetriveMode.Both && !rec.deleted)
                    dataGridView1.Rows[RowInd].DefaultCellStyle.BackColor = Color.Green;
                    else if (RM == HelperFunc.RetriveMode.Both && rec.deleted)
                        dataGridView1.Rows[RowInd].DefaultCellStyle.BackColor = Color.Red;
                    RowInd++;
                }



            }
            else if (radioButton2.Checked)
            {
                dataGridView1.Columns.Add("IDC", "ID");
                dataGridView1.Columns.Add("NAMEC", "NAME");
                dataGridView1.Columns.Add("JTC", "jobType");
              
                int RowInd = 0;
                var tt = g1.get_Staff(RM);
                label9.Text = tt.Count.ToString();
                foreach (var rec in tt)
                {
                    dataGridView1.Rows.Add(rec.ID, rec.Name, (HelperFunc.JopType)rec.jobType);

                    if (RM == HelperFunc.RetriveMode.Both && !rec.deleted)
                        dataGridView1.Rows[RowInd].DefaultCellStyle.BackColor = Color.Green;
                    else if (RM == HelperFunc.RetriveMode.Both && rec.deleted)
                        dataGridView1.Rows[RowInd].DefaultCellStyle.BackColor = Color.Red;
                    RowInd++;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDataBase(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!HasAform)
            {
                BTNindex = 1;
                pickIDBox.Enabled = true;
                pickIDBox.Visible = true;
                HasAform = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        void UpdateDataBase()
        {
            if (radioButton3.Checked)
                UpdateGridView(HelperFunc.RetriveMode.notDeleted);
            if (radioButton4.Checked)
                UpdateGridView(HelperFunc.RetriveMode.deleted);
            if (radioButton5.Checked)
                UpdateGridView(HelperFunc.RetriveMode.Both);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!HasAform)
            {
                if (radioButton1.Checked)
                {
                  
                        Add_member_box.Enabled = true;
                        Add_member_box.Visible = true;
                        HasAform = true; 

                    
                
                }

                else   if (radioButton2.Checked)
                {
                   
                        Add_staff_box .Enabled = true;
                        Add_staff_box.Visible = true;
                        HasAform = true;

                }

              
            }
        }

        private void BTN_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
               
                HelperFunc.MemType _type  = HelperFunc.MemType.Silver ;
                switch (memShip_selector.SelectedIndex)
                {
                    case 0:
                        _type = HelperFunc.MemType.Golden;
                        break;
                    case 1:
                        _type = HelperFunc.MemType.Silver;
                        break;
                    case 3:
                        _type = HelperFunc.MemType.VIP;
                        break;

                }
                Member m =   new Member(memID.Text, memName.Text, _type, dateTimePicker1.Value, dateTimePicker2.Value); 
                if (!Update){
                           g1.Add(m);
                            MessageBox.Show("member added successfully ! "); 
                }
                else {
                    if (g1.Update(picked_id.Text,m))
                         MessageBox.Show("member Updated successfully ! ");
                    else
                             MessageBox.Show("Not Found Or empty ");
                }


                }
                Add_member_box.Enabled = false;
                Add_member_box.Visible = false;
                HasAform =false;


                UpdateDataBase
                    ();
                }
                


        private void BTN2_Click(object sender, EventArgs e)
        {
             if ( radioButton2.Checked == true){
                HelperFunc.JopType _type = HelperFunc.JopType.JuniorCoach;
                switch (  jop_selector .SelectedIndex)
                {
                    case 0:
                        _type = HelperFunc.JopType.Secretary;
                        break;
                    case 1:
                        _type = HelperFunc.JopType.JuniorCoach;
                        break;
                    case 3:
                        _type = HelperFunc.JopType.SeniorCoach;
                        break;

                }
                Staff s = new Staff(staffID.Text, staffName.Text, _type);
                 if (!Update){
                           g1.Add(s);
                            MessageBox.Show("Staff added successfully ! "); 
                }
                else {
                    if (g1.Update(picked_id.Text,s))
                         MessageBox.Show("Staff Updated successfully ! ");
                    else
                             MessageBox.Show("Not Found Or empty ");
                }


              
                Add_staff_box.Enabled = false;
                Add_staff_box.Visible = false;
                HasAform = false;
                UpdateDataBase
                   ();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("DO u want to clear memberFile ?? ", "Warrning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                memberFS.SetLength(0);
                MessageBox.Show("MemberFile Cleared successfully ! ");
            }
          

        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("DO u want to clear staffFile ?? ", "Warrning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                StaffFS.SetLength(0);
                MessageBox.Show("StaffFile Cleared successfully ! ");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Add_member_box.Enabled = false;
            Add_member_box.Visible = false;
            HasAform = false; 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Add_staff_box.Enabled = false;
            Add_staff_box.Visible = false;
            HasAform = false; 
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (!HasAform)
            {
                BTNindex = 2;
                pickIDBox.Enabled = true;
                pickIDBox.Visible = true;
                HasAform = true;
                UpdateDataBase
                ();
            }


        }

        private void button12_Click(object sender, EventArgs e)
        {
            bool deleted = false ;
           
            switch (BTNindex)
            {
                case 2 :
                    if (radioButton1.Checked)
                        deleted = g1.DeleteMember(picked_id.Text);
                    else if (radioButton2.Checked)
                        deleted = g1.DeleteStaff(picked_id.Text);
                    MessageBox.Show((deleted) ? "deleted Suc" : " not found Or empty");
                    break; 



                case 1 : 
                    if(radioButton1.Checked)
                    {
                        Member m  = g1.SearchForMember (picked_id.Text ,HelperFunc.RetriveMode.notDeleted).Value;
                        if (m != null)
                            MessageBox.Show(m.ID + "-" + m.Name + "-" + m.mem_type + "-" + m.MemStartDate.ToShortDateString() + "-" + m.MemEndDate.ToShortDateString());
                        else MessageBox.Show("not Found Or empty "); 
                    }

                    else if (radioButton2.Checked)
                        {
                            Staff m = g1.SearchForStaff(picked_id.Text, HelperFunc.RetriveMode.notDeleted).Value;
                            if (m != null)
                                MessageBox.Show(m.ID + "-" + m.Name + "-" + m.jobType);
                            else MessageBox.Show("not Found Or empty ");
                        }
                    break;


                case 3  : 
                        if(radioButton1.Checked)
                         MessageBox.Show( (g1.RestoreMember(picked_id.Text) ? "Restored Suc !" : "not Found "  ));
                        else if (radioButton2.Checked)
                           MessageBox.Show( (g1.RestoreStaff(picked_id.Text) ? "Restored Suc !" : "not Found "  ));         
                        break;


                case 4 :

                      MessageBox.Show((g1.ExtenedMemberShip(picked_id.Text , 1) )? "Extended Suc !" : "not Found "  );         


                        break;
                case 5 :

                        if (radioButton1.Checked)
                        {

                            Add_member_box.Enabled = true;
                            Add_member_box.Visible = true;
                            Update = true;


                            Member m = g1.SearchForMember(picked_id.Text, HelperFunc.RetriveMode.notDeleted).Value;
                            if (m != null)
                            {
                                memID.Text = m.ID;
                                memName.Text = m.Name;
                                memShip_selector.SelectedItem = m.mem_type.ToString();
                                dateTimePicker1.Value = m.MemStartDate;
                                dateTimePicker2.Value = m.MemEndDate;
                            }
                            else
                                MessageBox.Show("not Found Or empty ");   

                        }

                        else if (radioButton2.Checked)
                        {

                            Add_staff_box.Enabled = true;
                            Add_staff_box.Visible = true;
                            Update = true;


                            Staff m = g1.SearchForStaff(picked_id.Text, HelperFunc.RetriveMode.notDeleted).Value;
                            if (m != null)
                            {
                                 staffID.Text = m.ID;
                            staffName.Text = m.Name;
                                jop_selector.SelectedItem = m.jobType.ToString();
                               
                            }
                            else
                                MessageBox.Show("not Found Or empty ");   

                        }

                        break; 
                default : 
                    break; 
            }


            pickIDBox.Enabled = false;
            pickIDBox.Visible = false;
            if (Update == true) 
                HasAform = true;
            else 
                HasAform = false;


            UpdateDataBase
                ();





        }

        private void button11_Click(object sender, EventArgs e)
        {
            pickIDBox.Enabled = false;
            pickIDBox.Visible = false;
            HasAform = false; 

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!HasAform)
            {
                BTNindex = 3;
                pickIDBox.Enabled = true;
                pickIDBox.Visible = true;
                HasAform = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            if (!HasAform)
            {
                BTNindex = 4;
                pickIDBox.Enabled = true;
                pickIDBox.Visible = true;
                HasAform = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {


            if (!HasAform)
            {
                BTNindex = 5;
                pickIDBox.Enabled = true;
                pickIDBox.Visible = true;
                HasAform = true;
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(
                     "Tell Us that you have Used It :)  ! Open Developer FacebookPage ?   ","AboutDeveloper", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk
                ) == DialogResult.Yes)

                System.Diagnostics.Process.Start("https://www.facebook.com/mohedsh");
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataBase
                ();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataBase
                ();
        }
        
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataBase();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataBase();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataBase();
        }

        
    }
}
