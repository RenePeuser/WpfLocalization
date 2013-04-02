using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Demo.Controls
{
    /// <summary>
    /// A <see cref="ContentControl"/> that exposes a non-dependency property.
    /// </summary>
    [LocalizabilityAttribute(LocalizationCategory.Label)]
    public class TestControl : Label
    {
        /// <summary>
        /// Gets or sets the content of the control.
        /// </summary>
        /// <value>The content of the control.</value>
        public string StringContent
        {
            get
            {
                return Convert.ToString(Content);
            }
            set
            {
                Content = value;
            }
        }
    }
}
