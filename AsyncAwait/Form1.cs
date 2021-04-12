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

        // 동기 실행
        // 작업이 끝날때 까지 UI가 동작하지 않는다
        private void btnSync_Click(object sender, EventArgs e) {
            var jobResult = DoJob("sync");
            this.Text = jobResult;
        }

        // 비동기 실행
        // 작업을 비동기로 실행하고 제어권은 ui thread로 넘겨서 작업중에도 UI가동작
        // 비동기 작업이 완료 되면 await 구문으로 작업의 결과가 받아지고 await 구문 이후 코드를 실행
        private async void btnAsync_Click(object sender, EventArgs e) {
            var jobResult = await Task.Run(() => DoJob("async await"));
            this.Text = jobResult;
        }

        // Task 를 직접 생성하지 않고 별도의 함수에서 생성
        private Task<string> DoJobAsync(string jobName) {
            return Task.Run(() => DoJob(jobName));
        }

        private async void btnAsync2_Click(object sender, EventArgs e) {
            var jobResult = await DoJobAsync("async await 2");
            this.Text = jobResult;
        }

        // async/await 가 아닌 Task.ContinueWith 로 동일 동작 구현
        private void btnTask_Click(object sender, EventArgs e) {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Run(() => DoJob("task continueWith"))
                .ContinueWith(task => this.Text = task.Result, context);    // context를 넣어주지 않으면 cross thread 에러 발생
        }
    }
}
