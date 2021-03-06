﻿using System;
using OpenQA.Selenium;
using Geeks.Pangolin.Core.Exception;
using Geeks.Pangolin.Core.Helper;

namespace Geeks.Pangolin.Helper.Command
{
    public abstract class CommandBase
    {
        #region [Property]
        const int ExecutedCount = 5;

        #endregion

        #region [Constructor]

        #endregion

        #region [Protected Method]

        protected abstract void Reset();

        protected void RunCommand(UIContext.UIContext uiContext, Action action)
        {
            var executedTimes = 1;
            var timeout = 1;

            Func<bool> isCommandRun = () =>
            {
                try
                {
                    action();
                    Reset();
                    return true;
                }
                catch (System.Exception ex)
                {
                    if (ex is UnhandledAlertException)
                        uiContext.WebDriverService.AcceptAnyAlert();
                    else if (ex is OpenQA.Selenium.StaleElementReferenceException ||
                        ex is Core.Exception.StaleElementReferenceException ||
                        ex is NoSuchElementException ||
                        ex is NoSuchFrameException ||
                        ex.GetType().Name == "ElementNotVisibleException" ||
                        ex.GetType().Name == "NoSuchFrameException"
                    )
                    {
                        if (executedTimes == ExecutedCount)
                            throw new CommandException(ex.Message);
                        System.Threading.Thread.Sleep(timeout * 500);
                        timeout = (timeout * 2);
                        executedTimes++;
                    }
                    else if (ex is CommandException)
                    {
                        uiContext.TestRunner.IsTestFailed = true;
                        throw new System.Exception(ex.Message);
                    }
                    else
                    {
                        if (executedTimes == ExecutedCount)
                        {
                            uiContext.TestRunner.IsTestFailed = true;
                            throw new CommandException(ex.Message);

                        }
                        System.Threading.Thread.Sleep(timeout * 500);
                        timeout = (timeout * 2);
                        executedTimes++;
                    }
                }
                return false;
            };
            isCommandRun.RepeatUntilCountNoException(10);
        }

        protected T RunCommand<T>(UIContext.UIContext uiContext, Func<T> func)
        {
            var executedTimes = 1;
            var timeout = 1;
            var element = default(T);

            Func<bool> isCommandRun = () =>
            {
                try
                {
                    element = func();
                    Reset();
                    return true;
                }
                catch (System.Exception ex)
                {
                    if (ex is UnhandledAlertException)
                        uiContext.WebDriverService.AcceptAnyAlert();
                    else if (ex is OpenQA.Selenium.StaleElementReferenceException ||
                        ex is Core.Exception.StaleElementReferenceException ||
                        ex is NoSuchElementException ||
                        ex is NoSuchFrameException ||
                        ex.GetType().Name == "ElementNotVisibleException" ||
                        ex.GetType().Name == "NoSuchFrameException"
                        )
                    {
                        if (executedTimes == ExecutedCount)
                            throw new CommandException(ex.Message);
                        System.Threading.Thread.Sleep(timeout * 500);
                        timeout = (timeout * 2);
                        executedTimes++;
                    }
                    else if (ex is CommandException)
                    {
                        uiContext.TestRunner.IsTestFailed = true;
                        throw new System.Exception(ex.Message);
                    }
                    else
                    {
                        if (executedTimes == ExecutedCount)
                        {
                            uiContext.TestRunner.IsTestFailed = true;
                            throw new CommandException(ex.Message);
                        }
                        System.Threading.Thread.Sleep(timeout * 500);
                        timeout = (timeout * 2);
                        executedTimes++;
                    }
                }
                return false;
            };
            isCommandRun.RepeatUntilCountNoException(10);

            return element;
        }

        #endregion

        #region [Private Method]

        #endregion
    }
}
