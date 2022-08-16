namespace Fake_UI_2
{
    public partial class Form1 : Form
    {

        private void set_beginner_mode()
        {
            tabControl1.TabPages.Remove(tabPage2); // devide addres
            tabControl1.TabPages.Remove(tabPage4); // devide configuration
            tabControl1.TabPages.Remove(tabPage9); // Automation LM
            tabControl2.TabPages.Remove(tabPage8); // devide configuration -> Events


            comboBox2.Items.Clear();
            comboBox2.Items.Add("Output 1");
            comboBox2.Items.Add("Output 2");
            comboBox2.Items.Add("Output 3");
            comboBox2.Items.Add("Output 4");
            comboBox2.Items.Add("Output 5");
            comboBox2.Items.Add("Output 6");
            comboBox2.SelectedIndex = 0;


            comboBox3.Items.Clear();
            comboBox3.Items.Add("Output - relay ON");
            comboBox3.Items.Add("Output - relay ON");
            comboBox3.Items.Add("Output - relay TOGGLE");
            comboBox3.SelectedIndex = 0;

            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;

            textBox10.Visible = false;
            textBox11.Visible = false;
            textBox12.Visible = false;


            label30.Visible = false;
            textBox13.Visible = false;

            //Eventy:

            comboBox10.Items.Clear();
            comboBox10.Items.Add("Mod - id: Kuchnia1 MAC 00:11:22:33:44:51");
            comboBox10.Items.Add("Mod - id: Kuchnia2 MAC 00:11:22:33:44:52");
            comboBox10.Items.Add("Mod - id: Kuchnia3 MAC 00:11:22:33:44:53");
            comboBox10.Items.Add("Mod - id: Kuchnia4 MAC 00:11:22:33:44:54");
            comboBox10.SelectedIndex = 0;


            /*            comboBox11.Items.Clear();
                        comboBox11.Items.Add("Input");
                        comboBox11.Items.Add("Touch panel");
                        comboBox11.Items.Add("Output");
                        comboBox11.SelectedIndex = 0;
            */

            comboBox12.Items.Clear();
            comboBox12.Items.Add("Input - short click");
            comboBox12.Items.Add("Input - long click");
            comboBox12.Items.Add("Input - very long click");
            comboBox12.Items.Add("Input - release");
            comboBox12.Items.Add("Relay output - ON");
            comboBox12.Items.Add("Relay output - OFF");


            comboBox12.Items.Add("Touch panel - key  1 - short click");
            comboBox12.Items.Add("Touch panel - key  2 - short click");
            comboBox12.Items.Add("Touch panel - key  3 - short click");
            comboBox12.Items.Add("Touch panel - key  4 - short click");
            comboBox12.Items.Add("Touch panel - key  5 - short click");
            comboBox12.Items.Add("Touch panel - key  6 - short click");
            comboBox12.Items.Add("Touch panel - key  7 - short click");
            comboBox12.Items.Add("Touch panel - key  8 - short click");
            comboBox12.Items.Add("Touch panel - key  9 - short click");
            comboBox12.Items.Add("Touch panel - key 10 - short click");

            comboBox12.Items.Add("Touch panel - key  1 - long click");
            comboBox12.Items.Add("Touch panel - key  2 - long click");
            comboBox12.Items.Add("Touch panel - key  3 - long click");
            comboBox12.Items.Add("Touch panel - key  4 - long click");
            comboBox12.Items.Add("Touch panel - key  5 - long click");
            comboBox12.Items.Add("Touch panel - key  6 - long click");
            comboBox12.Items.Add("Touch panel - key  7 - long click");
            comboBox12.Items.Add("Touch panel - key  8 - long click");
            comboBox12.Items.Add("Touch panel - key  9 - long click");
            comboBox12.Items.Add("Touch panel - key 10 - long click");         

            comboBox12.SelectedIndex = 0;



        }

        private void set_advanced_mode()
        {
            tabControl2.TabPages.Remove(tabPage8); // devide configuration -> Events

            if (!tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Insert(1, tabPage2);
            if (!tabControl1.TabPages.Contains(tabPage4)) tabControl1.TabPages.Insert(3, tabPage4);

            comboBox2.Items.Clear();
            comboBox2.Items.Add("Output 1");
            comboBox2.Items.Add("Output 2");
            comboBox2.Items.Add("Output 3");
            comboBox2.Items.Add("Output 4");
            comboBox2.Items.Add("Output 5");
            comboBox2.Items.Add("Output 6");
            comboBox2.Items.Add("Shutter 1");
            comboBox2.Items.Add("Shutter 2");
            comboBox2.Items.Add("Shutter 3");
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Clear();
            comboBox3.Items.Add("Output - relay ON");
            comboBox3.Items.Add("Output - relay ON");
            comboBox3.Items.Add("Output - relay TOGGLE");
            comboBox3.Items.Add("Shutter - open / stop");
            comboBox3.Items.Add("Shutter - close / stop");
            comboBox3.Items.Add("Shutter - open / stop / close");
            comboBox3.Items.Add("Shutter - lamels open impuls");
            comboBox3.Items.Add("Shutter - lamels close impuls");
           // comboBox3.Items.Add("Group event");
            comboBox3.SelectedIndex = 0;

            label27.Visible = true;
            label28.Visible = true;
            label29.Visible = true;

            textBox10.Visible = true;
            textBox11.Visible = true;
            textBox12.Visible = true;


            comboBox12.Items.Clear();
            comboBox12.Items.Add("Input - short click");
            comboBox12.Items.Add("Input - long click");
            comboBox12.Items.Add("Input - very long click");
            comboBox12.Items.Add("Input - release");
            comboBox12.Items.Add("Relay output - ON");
            comboBox12.Items.Add("Relay output - OFF");

            comboBox12.Items.Add("Touch panel - key  1 - short click");
            comboBox12.Items.Add("Touch panel - key  2 - short click");
            comboBox12.Items.Add("Touch panel - key  3 - short click");
            comboBox12.Items.Add("Touch panel - key  4 - short click");
            comboBox12.Items.Add("Touch panel - key  5 - short click");
            comboBox12.Items.Add("Touch panel - key  6 - short click");
            comboBox12.Items.Add("Touch panel - key  7 - short click");
            comboBox12.Items.Add("Touch panel - key  8 - short click");
            comboBox12.Items.Add("Touch panel - key  9 - short click");
            comboBox12.Items.Add("Touch panel - key 10 - short click");

            comboBox12.Items.Add("Touch panel - key  1 - long click");
            comboBox12.Items.Add("Touch panel - key  2 - long click");
            comboBox12.Items.Add("Touch panel - key  3 - long click");
            comboBox12.Items.Add("Touch panel - key  4 - long click");
            comboBox12.Items.Add("Touch panel - key  5 - long click");
            comboBox12.Items.Add("Touch panel - key  6 - long click");
            comboBox12.Items.Add("Touch panel - key  7 - long click");
            comboBox12.Items.Add("Touch panel - key  8 - long click");
            comboBox12.Items.Add("Touch panel - key  9 - long click");
            comboBox12.Items.Add("Touch panel - key 10 - long click");

            comboBox12.Items.Add("Shutter output - opening start");
            comboBox12.Items.Add("Shutter output - closing start");
            comboBox12.Items.Add("Shutter output - stop");

            comboBox12.SelectedIndex = 0;




        }

        private void set_expert_mode()
        {
            set_advanced_mode();

            if (!tabControl1.TabPages.Contains(tabPage9)) tabControl1.TabPages.Insert(5, tabPage9);
            if (!tabControl2.TabPages.Contains(tabPage8)) tabControl2.TabPages.Insert(2, tabPage8);

            label30.Visible = true;
            textBox13.Visible = true;


        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            set_beginner_mode();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_advanced_Click(object sender, EventArgs e)
        {
            set_advanced_mode();
            
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            set_expert_mode();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            set_beginner_mode();
        }
    }
}