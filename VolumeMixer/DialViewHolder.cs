using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace VolumeMixer
{
    class DialViewHolder
    {
        public Label label { get; }
        public Image image { get; }
        public ComboBox combobox { get; }
        public ProgressBar progressbar { get; }

        public DialViewHolder(Label label, Image image, ComboBox combobox, ProgressBar progressbar)
        {
            this.label = label;
            this.image = image;
            this.combobox = combobox;
            this.progressbar = progressbar;
        }
    }
}
