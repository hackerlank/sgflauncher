﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            timer2.Start();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Opacity += .03;
            if (this.Opacity == 1)
            {
                timer2.Stop();
                
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
