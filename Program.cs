using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Retry
{
    /// <summary>
    /// リトライ時のルール（処理回数や、待機時間)
    /// </summary>
    public struct RetryPolicy
    {
        /// <summary>
        /// リトライ回数
        /// </summary>
        public int RetryNumber { get; set; }
        /// <summary>
        /// 処理停止時間
        /// </summary>
        public int RetrySleep { get; set; }
    }
    /// <summary>
    /// リトライ処理が必要な所で指定された条件によってリトライを大なうためのクラス
    /// </summary>
    public class RetryExecutor
    {
        private readonly RetryPolicy _policy;

        public RetryExecutor()
        {
            RetryPolicy policy = new RetryPolicy();
            policy.RetryNumber = 5;
            policy.RetrySleep = 5000;
            _policy = policy;
        }

        public virtual void Execute(Action<string,string> CreateFtpSite, string virtualPath,string physicalPath)
        {
            for (var retryNum = 1; retryNum <= _policy.RetryNumber + 1; retryNum++)
            {
                try
                {
                    CreateFtpSite(virtualPath, physicalPath);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine((ex.ToString()));

                    if (retryNum > _policy.RetryNumber)
                    {
                        throw new Exception(string.Format("リトライ回数をオーバーしたため、処理を終了します。最大リトライ回数：{0}", (_policy.RetryNumber).ToString()));
                    }

                    Console.WriteLine(string.Format("一定時間[{0}ミリ秒]待機した後、リトライします。リトライ回数：{1}", _policy.RetrySleep.ToString(), retryNum.ToString()));
                    Thread.Sleep(_policy.RetrySleep);
                }
            }
        }
    
      
    }
    public class sample
    {
        /// <summary>
        /// 例外を起こすサンプルメソッド
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="b"></param>
        public void methodd(string d, string e)
        {
            int[] num = new int[3];
            for (var i = 0; i < 4; i++)
            {
                num[i] = 1;
            }
            //Console.WriteLine("テスト");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            RetryExecutor retry = new RetryExecutor();
            sample sm = new sample();
            Action<string,string> act3 = sm.methodd;
            retry.Execute(act3,"ss","dd");
            Console.ReadLine();
        }
    }
}
