using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntuislemeV2.form
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            InitializeHelpContent();
            this.AutoSize = true;
        }

        private void InitializeHelpContent()
        {
            this.Text = "Help - Shortcuts";
            this.Size = new Size(400, 300);

            Label helpLabel = new Label
            {
                Text = "Keyboard Shortcuts:\n\n" +
                       "Ctrl + F: Show the selected image in full resolution.\n" +
                       "Ctrl + S: Save the selected image.\n\n"
                       
                       ,
                    
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("Arial", 10)
            };

            this.Controls.Add(helpLabel);
        }
    }
}


