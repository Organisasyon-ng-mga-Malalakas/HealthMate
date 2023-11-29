// <auto-generated/>
using HealthMate.Services.HttpServices.User.ForgotPassword.Confirm.Item;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace HealthMate.Services.HttpServices.User.ForgotPassword.Confirm {
    /// <summary>
    /// Builds and executes requests for operations under \user\forgot-password\confirm
    /// </summary>
    public class ConfirmRequestBuilder : BaseRequestBuilder {
        /// <summary>Gets an item from the HealthMate.Services.HttpServices.user.forgotPassword.confirm.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        public WithHashItemRequestBuilder this[string position] { get {
            var urlTplParams = new Dictionary<string, object>(PathParameters);
            urlTplParams.Add("hash", position);
            return new WithHashItemRequestBuilder(urlTplParams, RequestAdapter);
        } }
        /// <summary>
        /// Instantiates a new ConfirmRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ConfirmRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/user/forgot-password/confirm", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new ConfirmRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ConfirmRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/user/forgot-password/confirm", rawUrl) {
        }
    }
}
