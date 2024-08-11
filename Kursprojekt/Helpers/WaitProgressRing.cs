using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursprojekt.Helpers
{
    public class WaitProgressRing : IDisposable
    {
        private ProgressRing progressRing;

        public WaitProgressRing(ProgressRing progressRing)
        {
            this.progressRing = progressRing;
            this.progressRing.Visibility = Visibility.Visible;
        }

        public void Dispose()
        {
            this.progressRing.Visibility = Visibility.Hidden;
        }
    }
}
