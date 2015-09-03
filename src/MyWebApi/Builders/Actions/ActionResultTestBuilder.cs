﻿namespace MyWebApi.Builders.Actions
{
    using System;
    using System.Web.Http;
    using Base;
    using Common.Extensions;
    using Contracts.Actions;
    using Exceptions;
    using ShouldHave;
    using ShouldReturn;
    using Utilities;

    /// <summary>
    /// Used for building the action result which will be tested.
    /// </summary>
    /// <typeparam name="TActionResult">Result from invoked action in ASP.NET Web API controller.</typeparam>
    public class ActionResultTestBuilder<TActionResult>
        : BaseTestBuilderWithActionResult<TActionResult>, IActionResultTestBuilder<TActionResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResultTestBuilder{TActionResult}" /> class.
        /// </summary>
        /// <param name="controller">Controller on which the action will be tested.</param>
        /// <param name="actionName">Name of the tested action.</param>
        /// <param name="caughtException">Caught exception during the action execution.</param>
        /// <param name="actionResult">Result from the tested action.</param>
        public ActionResultTestBuilder(
            ApiController controller,
            string actionName,
            Exception caughtException,
            TActionResult actionResult)
            : base(controller, actionName, caughtException, actionResult)
        {
        }

        /// <summary>
        /// Used for testing action attributes and model state.
        /// </summary>
        /// <returns>Should have test builder.</returns>
        public IShouldHaveTestBuilder<TActionResult> ShouldHave()
        {
            return new ShouldHaveTestBuilder<TActionResult>(this.Controller, this.ActionName, this.CaughtException, this.ActionResult);
        }

        /// <summary>
        /// Used for testing whether action throws exception.
        /// </summary>
        /// <returns>Should throw test builder.</returns>
        public IShouldThrowTestBuilder ShouldThrow()
        {
            if (this.CaughtException == null)
            {
                throw new ActionCallAssertionException(string.Format(
                    "When calling {0} action in {1} thrown exception was expected, but in fact none was caught.",
                    this.ActionName,
                    this.Controller.GetName()));
            }

            return new ShouldThrowTestBuilder(this.Controller, this.ActionName, this.CaughtException);
        }

        /// <summary>
        /// Used for testing returned action result.
        /// </summary>
        /// <returns>Should return test builder.</returns>
        public IShouldReturnTestBuilder<TActionResult> ShouldReturn()
        {
            Validator.CheckForException(this.CaughtException);
            return new ShouldReturnTestBuilder<TActionResult>(this.Controller, this.ActionName, this.CaughtException, this.ActionResult);
        }
    }
}
