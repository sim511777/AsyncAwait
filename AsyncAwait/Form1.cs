using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAwait {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private string DoJob(string jobName) {
            var st = DateTime.Now;
            for (int i = 0; i < 5; i++) {
                Thread.Sleep(1000);
            }
            var dt = DateTime.Now - st;
            return $"{jobName} - {dt.TotalSeconds:f2} sec";
        }

        private void btnSync_Click(object sender, EventArgs e) {
            var jobResult = DoJob("sync");
            this.Text = jobResult;
        }

        private async void btnAsync_Click(object sender, EventArgs e) {
            var jobResult = await Task.Run(() => DoJob("async"));
            // await를 만나면 작업을 비동기 적으로 실행하고 제어권을 호출자에게 넘겨준다.
            // 비동기 작업 완료되면 결과값을 리턴하고 제어권은 가져와서 이후의 코드를 실행한다.
            this.Text = jobResult;
        }
    }
}
