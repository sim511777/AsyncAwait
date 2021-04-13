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

        // 오래 걸리는 함수
        private string DoJob(string jobName) {
            var st = DateTime.Now;
            for (int i = 0; i < 5; i++) {
                Thread.Sleep(1000);
            }
            var dt = DateTime.Now - st;
            return $"{jobName} - {dt.TotalSeconds:f2} sec";
        }

        // 오래 걸리는 함수를 비동기 실행하고 Task 리턴
        // Task.Wait() 로 작업 완료 대기
        // Task.Result 로 리턴값 조회
        private Task<string> DoJobAsync(string jobName) {
            return Task.Run(() => DoJob(jobName));
        }

        // 동기 실행
        // 작업이 끝날때 까지 UI가 동작하지 않는다
        private void btnSync_Click(object sender, EventArgs e) {
            var jobName = "Run sync";

            var jobResult = DoJob(jobName);
            
            lblResult.Text = jobResult;
        }

        // 비동기 실행
        // 작업을 비동기로 실행하고 제어권은 ui thread로 넘겨서 작업중에도 UI가동작
        // 비동기 작업이 완료 되면 await 구문으로 작업의 결과가 받아지고 await 구문 이후 코드를 실행
        private async void btnAsync_Click(object sender, EventArgs e) {
            var jobName = "Run async/await";

            var jobResult = await DoJobAsync(jobName);
            //var jobResult = await Task.Run(() => DoJob(jobName));
            
            lblResult.Text = jobResult;
        }

        // async/await 가 아닌 Task.ContinueWith 로 동일 동작 구현
        private void btnTask_Click(object sender, EventArgs e) {
            var jobName = "Run Task";
            
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Run(() => DoJob(jobName))
                .ContinueWith(task => lblResult.Text = task.Result, context);    // context를 넣어주지 않으면 cross thread 에러 발생
        }
    }
}
