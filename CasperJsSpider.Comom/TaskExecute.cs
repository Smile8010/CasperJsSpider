using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Comom
{
    /// <summary>
    /// 任务执行类（可同时开始一定数量的任务，进行处理队列数据）
    /// </summary>
    /// <typeparam name="T">要处理的类型</typeparam>
    public class TaskExecute<T>
    {
        /// <summary>
        /// 线程安全的队列
        /// </summary>
        private ConcurrentQueue<T> _queue_;

        /// <summary>
        /// 每一项的执行方法
        /// </summary>
        private Action<T> _execuAction;

        /// <summary>
        /// 是否等待所有处理结束
        /// </summary>
        private bool _isWaited;

        /// <summary>
        /// 用于处理的任务集合
        /// </summary>
        private IList<Task> _taskList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execuAction">每项统一执行的方法</param>
        /// <param name="taskCount">开启的任务数</param>
        public TaskExecute(Action<T> execuAction, int taskCount) : this(execuAction, taskCount, true) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execuAction">每项统一执行的方法</param>
        public TaskExecute(Action<T> execuAction) : this(execuAction, 2, true) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execuAction">每项统一执行的方法</param>
        /// <param name="taskCount">开启的任务数</param>
        /// <param name="isWaited">是否等待所有任务线程处理结束</param>
        public TaskExecute(Action<T> execuAction, int taskCount, bool isWaited)
        {
            _queue_ = new ConcurrentQueue<T>();
            _isWaited = isWaited;
            _execuAction = execuAction;
            if (taskCount <= 0)
            {
                taskCount = 1;
            }
            _taskList = new List<Task>();
            while (taskCount > 0)
            {
                _taskList.Add(new Task(DoAction));
                taskCount--;
            }
        }

        /// <summary>
        /// 添加队列数据
        /// </summary>
        /// <param name="entity"></param>
        public void AddQueue(T entity)
        {
            _queue_.Enqueue(entity);
        }

        /// <summary>
        /// 任务启动
        /// </summary>
        public void Run()
        {
            foreach (Task t in _taskList)
            {
                t.Start();
            }
            if (_isWaited)
            {
                Task.WaitAll(_taskList.ToArray());
            }
        }

        /// <summary>
        /// 任务执行
        /// </summary>
        private void DoAction()
        {
            T entity;
            while (_queue_.TryDequeue(out entity))
            {
                _execuAction(entity);
            }
        }
    }
}
